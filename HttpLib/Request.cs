using System;
using System.IO;
using System.Net;

namespace HttpLib
{
    public delegate void ConnectionIssue(WebException ex);

    public static class Request
    {
        public static event ConnectionIssue ConnectFailed = delegate { };

        private static readonly CookieContainer cookies = new CookieContainer();
                
        #region get

        /// <summary>
        /// performs a http get operation
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Get(string url, Action<string> successCallback)
        {
            Get(url, new { }, StreamToStringCallback(successCallback));
        }

        /// <summary>
        /// performs a http get operation with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Get(string url, object parameters, Action<string> successCallback)
        {
            Get(url, parameters, StreamToStringCallback(successCallback), webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http get operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Get(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            Get(url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http get request
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Get(string url, Action<WebHeaderCollection, Stream> successCallback)
        {
            Get(url, new {}, successCallback);
        }

        /// <summary>
        /// performs a http get request with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Get(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            Get(url, parameters, successCallback, (webEx) =>
                {
                    ConnectFailed(webEx);
                });
        }

        /// <summary>
        /// performs a http get request with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Get(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            var b = new UriBuilder(url);
            
            if (parameters != null)
            {
                if (!string.IsNullOrWhiteSpace(b.Query))
                    b.Query = b.Query.Substring(1) + "&" + Utils.SerializeQueryString(parameters);
                else
                    b.Query =Utils.SerializeQueryString(parameters);
            }

            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Get, b.Uri.ToString(), new { }, successCallback, failCallback);
        }

        #endregion

        #region head

        /// <summary>
        /// performs a http head operation
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Head(string url, Action<string> successCallback)
        {
            Head(url, new { }, StreamToStringCallback(successCallback));
        }

        /// <summary>
        /// performs a http head operation with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Head(string url, object parameters, Action<string> successCallback)
        {
            Head(url, parameters, StreamToStringCallback(successCallback), webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http head operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Head(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            Head(url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http head operation
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Head(string url, Action<WebHeaderCollection, Stream> successCallback)
        {
            Head(url, new { }, successCallback);
        }

        /// <summary>
        /// performs a http head operation with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">a function that is called on success</param>
        public static void Head(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            Head(url, parameters, successCallback, webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http head operation with parameters and a function that is called on failure
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">kv array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Head(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            var b = new UriBuilder(url);
            if (parameters.GetType().GetProperties().Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(b.Query))
                    b.Query = b.Query.Substring(1) + "&" + Utils.SerializeQueryString(parameters);
                else
                    b.Query = Utils.SerializeQueryString(parameters);
            }

            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Head, b.Uri.ToString(), new { }, successCallback, failCallback);
        }

        #endregion

        #region post

        /// <summary>
        /// performs a http post request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Post(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, parameters, StreamToStringCallback(successCallback), webEx =>
                {
                    ConnectFailed(webEx);
                });
        }

        /// <summary>
        /// performs a http post request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Post(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, parameters, successCallback, webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http post request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Post(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, parameters,StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http post request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Post(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Post, url, parameters, successCallback, failCallback);
        }

        #endregion

        #region patch

        /// <summary>
        /// performs a http patch request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Patch(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, parameters, StreamToStringCallback(successCallback), webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http patch request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Patch(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, parameters, successCallback, webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http patch request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Patch(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http patch request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Patch(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Patch, url, parameters, successCallback, failCallback);
        }

        #endregion

        #region put

        /// <summary>
        /// performs a http put request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Put(string url, object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, parameters, StreamToStringCallback(successCallback), webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http put request on a target with parameters
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Put(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http put request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Put(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http put request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Put(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Put, url, parameters, successCallback, failCallback);
        }

        #endregion

        #region delete

        /// <summary>
        /// performs a http delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Delete(string url,object parameters, Action<string> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, parameters, StreamToStringCallback(successCallback), webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        public static void Delete(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, parameters, successCallback, (webEx) =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// performs a http delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Delete(string url, object parameters, Action<string> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, parameters, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// performs a http delete request with parameters and a fail function
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">array of parameters</param>
        /// <param name="successCallback">function that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Delete(string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            MakeRequest("application/x-www-form-urlencoded", HttpVerb.Delete, url, parameters, successCallback, failCallback);
        }

        #endregion

        #region upload

        /// <summary>
        /// upload an array of files to a remote host using the http post multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<string> successCallback)
        {
            Upload(url, parameters, files, successCallback, (webEx) =>
                {
                    ConnectFailed(webEx);
                });
        }

        /// <summary>
        /// upload an array of files to a remote host using the http post multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback)
        {
            Upload(url, parameters, files, successCallback, webEx =>
            {
                ConnectFailed(webEx);
            });
        }

        /// <summary>
        /// upload an array of files to a remote host using the http post multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<string> successCallback, Action<WebException> failCallback)
        {
            Upload(url, HttpVerb.Post, parameters, files, successCallback, failCallback);
        }

        /// <summary>
        /// upload an array of files to a remote host using the http post multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Upload(string url, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            Upload(url, HttpVerb.Post, parameters, files, successCallback, failCallback);
        }

        /// <summary>
        /// upload an array of files to a remote host using the http post or put multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="method">post or put</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Upload(string url, HttpVerb method, object parameters, NamedFileStream[] files, Action<string> successCallback, Action<WebException> failCallback)
        {
            Upload(url, method, parameters, files, StreamToStringCallback(successCallback), failCallback);
        }

        /// <summary>
        /// upload an array of files to a remote host using the http post or put multipart method
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="method">post or put</param>
        /// <param name="parameters">parmaters</param>
        /// <param name="files">an array of files</param>
        /// <param name="successCallback">funciton that is called on success</param>
        /// <param name="failCallback">function that is called on failure</param>
        public static void Upload(string url, HttpVerb method, object parameters, NamedFileStream[] files, Action<WebHeaderCollection, Stream> successCallback,Action<WebException> failCallback)
        {
            if (method != HttpVerb.Post && method != HttpVerb.Put)
                throw new ArgumentException("Request method must be POST or PUT");

            try
            {
                var boundary = RandomString(12);

                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = method.ToString();
                request.ContentType = "multipart/form-data, boundary=" + boundary;
                request.CookieContainer = cookies;
     
                request.BeginGetRequestStream(asynchronousResult =>
                {
                    var tmprequest = (HttpWebRequest)asynchronousResult.AsyncState;
                    var postStream = tmprequest.EndGetRequestStream(asynchronousResult);
                    var querystring = "\n";

                    foreach (var property in parameters.GetType().GetProperties())                    
                    {
                        querystring += "--" + boundary + "\n";
                        querystring += "content-disposition: form-data; name=\"" + System.Uri.EscapeDataString(property.Name) + "\"\n\n";
                        querystring += System.Uri.EscapeDataString(property.GetValue(parameters, null).ToString());
                        querystring += "\n";
                    }
                
                    var byteArray = System.Text.Encoding.UTF8.GetBytes(querystring);
                    postStream.Write(byteArray, 0, byteArray.Length);
                
                    var closing = System.Text.Encoding.UTF8.GetBytes("\n--" + boundary + "--\n");

                    foreach (var file in files)
                    {
                        var qsAppend = "--" + boundary + "\ncontent-disposition: form-data; name=\""+file.Name +"\"; filename=\"" + file.Filename + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                        var outBuffer = new MemoryStream();
                        outBuffer.Write(System.Text.Encoding.UTF8.GetBytes(qsAppend), 0, qsAppend.Length);

                        int bytesRead;
                        var buffer = new byte[4096];

                        while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outBuffer.Write(buffer, 0, bytesRead);
                        }
                        
                        outBuffer.Write(closing, 0, closing.Length);
                        outBuffer.Position = 0;
                        var tempBuffer = new byte[outBuffer.Length];
                        outBuffer.Read(tempBuffer, 0, tempBuffer.Length);
                        postStream.Write(tempBuffer, 0, tempBuffer.Length);
                        postStream.Flush();           
                    }
                    
                    postStream.Flush();
                    postStream.Dispose();

                    tmprequest.BeginGetResponse(ProcessCallback(successCallback, failCallback), tmprequest);
                }, request);
            }
            catch (WebException webEx)
            {
                failCallback(webEx);
            }
        }

        #endregion

        #region private

        private static Action<WebHeaderCollection, Stream> StreamToStringCallback(Action<string> stringCallback)
        {
            return (headers, resultStream) =>
            {
                using (var sr = new StreamReader(resultStream))
                {
                    stringCallback(sr.ReadToEnd());
                }
            };
        }

        private static void MakeRequest(string contentType, HttpVerb method, string url, object parameters, Action<WebHeaderCollection, Stream> successCallback, Action<WebException> failCallback)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters object cannot be null");

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url is empty");
            
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cookies;
                request.Method = method.ToString();
                
                switch (method)
                {
                    case HttpVerb.Delete:
                    case HttpVerb.Post:
                    case HttpVerb.Put:
                    case HttpVerb.Patch:
                        request.ContentType = contentType;
                        request.BeginGetRequestStream(callbackResult =>
                        {                 
                            var tmprequest = (HttpWebRequest)callbackResult.AsyncState;
                            var postStream = tmprequest.EndGetRequestStream(callbackResult);
                            var postbody = Utils.SerializeQueryString(parameters);
                            var byteArray = System.Text.Encoding.UTF8.GetBytes(postbody);
                            postStream.Write(byteArray, 0, byteArray.Length);
                            postStream.Flush();
                            postStream.Dispose();
                            tmprequest.BeginGetResponse(ProcessCallback(successCallback,failCallback), tmprequest);
                        }, request);
                        break;
                    case HttpVerb.Get:
                    case HttpVerb.Head:
                        request.BeginGetResponse(ProcessCallback(successCallback,failCallback), request);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(method), method, null);
                }
            }
            catch (WebException webEx)
            {
                failCallback(webEx);
            }
        }

        private static string RandomString(int length)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        private static AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
        {
            return callbackResult =>
            {
                var myRequest = (HttpWebRequest) callbackResult.AsyncState;

                try
                {
                    using (var myResponse = (HttpWebResponse) myRequest.EndGetResponse(callbackResult))
                    {
                        success(myResponse.Headers, myResponse.GetResponseStream());
                    }
                }
                catch (WebException webEx)
                {
                    if (ConnectFailed != null)
                        fail(webEx);
                }
            };
        }

        #endregion
    }
}
