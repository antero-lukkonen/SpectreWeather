namespace Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class Request
    {
        private readonly HttpClient httpClient;
        private readonly HttpRequestMessage httpRequest;
        private readonly IList<Func<HttpRequestMessage, Func<HttpRequestMessage, Task<HttpResponseMessage>>, Task<HttpResponseMessage>>> middleware = new List<Func<HttpRequestMessage, Func<HttpRequestMessage, Task<HttpResponseMessage>>, Task<HttpResponseMessage>>>();

        private Func<Response, Exception> getFailedException;

        private Request(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpRequest = new HttpRequestMessage();
        }

        public static Request WithClient(HttpClient httpClient) => new Request(httpClient);

        public Call Get() => this.Send(HttpMethod.Get, null);

        public Call Put(HttpContent content) => this.Send(HttpMethod.Put, content);

        public Call Post(HttpContent content) => this.Send(HttpMethod.Post, content);


        public Request Use(params Func<HttpRequestMessage, Func<HttpRequestMessage, Task<HttpResponseMessage>>, Task<HttpResponseMessage>>[] middlewareFuncs)
        {
            Array.ForEach(middlewareFuncs, this.middleware.Add);
            return this;
        }

        public Request WithHeader(string key, string value)
        {
            this.httpRequest.Headers.Add(key, value);
            return this;
        }

        public Request WithPath(string pathTemplate, params object[] pathTemplateValues)
        {
            this.httpRequest.RequestUri = new Uri(string.Format(pathTemplate, pathTemplateValues), UriKind.Relative);
            return this;
        }

        public Request WithFailException(Func<Response, Exception> getException)
        {
            this.getFailedException = getException;
            return this;
        }

        public Request WithFailException()
            => this.WithFailException(response => new RequestException(response));

        private Call Send(HttpMethod method, HttpContent content)
        {
            this.httpRequest.Content = content;
            this.httpRequest.Method = method;

            var sendAsync = this.ChainMiddleware(this.httpClient.SendAsync);

            return new Call(() => sendAsync(this.httpRequest), this.getFailedException);
        }

        private Func<HttpRequestMessage, Task<HttpResponseMessage>> ChainMiddleware(Func<HttpRequestMessage, Task<HttpResponseMessage>> sendAsync)
            => this.middleware.Reverse().Aggregate(sendAsync, (next, f) => req => f(req, next));
    }


    public class Call
    {
        private readonly Func<Task<HttpResponseMessage>> getResponseMessage;

        private readonly Func<Response, Exception> getFailedException;

        internal Call(Func<Task<HttpResponseMessage>> getResponseMessage, Func<Response, Exception> getFailedException)
        {
            this.getResponseMessage = getResponseMessage;
            this.getFailedException = getFailedException;
        }

        public Task<Response> ReceiveResponse(bool readBody = false)
            => Response.Create(this.getResponseMessage, this.getFailedException, readBody);

        public async Task<string> ReceiveBody()
            => (await this.ReceiveResponse(true)).Body;

        public async Task<HttpResponseHeaders> ReceiveHeaders()
            => (await this.ReceiveResponse()).Headers;

        public async Task<int> ReceiveCode()
            => (await this.ReceiveResponse()).Code;

        public async Task<bool> ReceiveIsSuccess()
            => (await this.ReceiveResponse()).IsSuccess;
    }


    public class Response
    {
        public HttpResponseMessage ResponseMessage { get; }

        public static async Task<Response> Create(Func<Task<HttpResponseMessage>> getResponseMessage, Func<Response, Exception> getFailedException, bool readBody)
        {
            var response = new Response(await getResponseMessage());
            if (!response.IsSuccess && getFailedException != null)
            {
                throw getFailedException(await response.ReadBody());
            }
            if (readBody)
            {
                return await response.ReadBody();
            }
            return response;
        }

        private Response(HttpResponseMessage responseMessage)
        {
            this.ResponseMessage = responseMessage;
            this.Headers = responseMessage.Headers;
        }

        public HttpResponseHeaders Headers { get; }

        public string Body { get; private set; }

        public int Code => (int)this.ResponseMessage.StatusCode;

        public string Text => this.ResponseMessage.ReasonPhrase;

        public bool IsSuccess => this.ResponseMessage.IsSuccessStatusCode;

        public async Task<Response> ReadBody()
        {
            this.Body = this.ResponseMessage.Content != null
                ? await this.ResponseMessage.Content.ReadAsStringAsync()
                : string.Empty;
            return this;
        }
    }

    public class RequestException : Exception
    {
        public Response Response { get; }

        public RequestException(string message, Response response)
            : base(message)
        {
            this.Response = response;
        }

        public RequestException(Response response) : this("Request failed", response)
        { }
    }
}
