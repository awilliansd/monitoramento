using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Monitoramento.WebUtility
{
    public class RestSharpClient : RestClient
    {
        public RestSharpClient(string baseUrl) : base(baseUrl)
        {
        }

        private RetryPolicy<IRestResponse> GetRetryPolicy()
        {
            return Policy
                .HandleResult<IRestResponse>(
                    r =>
                        r.Request.Method == Method.GET
                        && new List<int> { 500, 502, 503, 504, 404 }
                            .Contains((int)r.StatusCode))
                .OrResult(
                    r =>
                        r.Request.Method != Method.GET
                        && new List<int> { 500, 502, 503, 404 }
                            .Contains((int)r.StatusCode))
                .WaitAndRetry(4,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }

        private AsyncRetryPolicy<IRestResponse> AsyncGetRetryPolicy()
        {
            return Policy
                .HandleResult<IRestResponse>(
                    r =>
                        r.Request.Method == Method.GET
                        && new List<int> { 500, 502, 503, 504 }
                            .Contains((int)r.StatusCode))
                .OrResult(
                    r =>
                        r.Request.Method != Method.GET
                        && new List<int> { 500, 502, 503 }
                            .Contains((int)r.StatusCode))
                .WaitAndRetryAsync(4,
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }
    }
}