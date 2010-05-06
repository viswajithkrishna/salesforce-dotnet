/*
 * Salesforce API for .NET
 * 
 * Gaiaware also develops Gaia Ajax - the open source Ajax framework for ASP.NET
 * http://gaiaware.net
 * 
 * Copyright (c) 2009 Gaiaware AS.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Net;
using System.IO;

namespace Gaiaware.Salesforce.Utilities
{
    /// <summary>
    /// This class wraps an existing WebRequest, and will handle compressing the request, and decompressing the response as needed.
    /// 
    /// To use this with a Soap Client, create a new class that derives from the WSDL generated class and override GetWebRequest, 
    /// its implementation should simply be 
    ///		return new GzipWebRequest(base.GetWebRequest(uri));
    /// or if you want to compress the request message as well as the response, do this
    ///     return new GzipWebRequest(base.GetWebRequest(uri), true, true);
    /// 
    /// Then when using the web service, remember to create instances of the derived class, rather than the generated class    
    /// </summary>
    public class GzipWebRequest : WebRequest
    {
        internal const string GZIP = "gzip";

        /// <summary>
        /// This constructor will send an uncompressed request, and indicate that we can accept a compressed response.
        /// You should be able to use this anywhere to get automatic support for handling compressed responses
        /// </summary>
        /// <param name="wrappedRequest"></param>
        public GzipWebRequest(WebRequest wrappedRequest) : this(wrappedRequest, false, true)
        {
        }

        /// <summary>
        /// This constructor allows to indicate if you want to compress the request, and if you want to indicate that you can handled a compressed response
        /// </summary>
        /// <param name="wrappedRequest">The WebRequest we're wrapping.</param>
        /// <param name="compressRequest">if true, we will gzip the request message.</param>
        /// <param name="acceptCompressedResponse">if true, we will indicate that we can handle a gzip'd response, and decode it if we get a gziped response.</param>
        public GzipWebRequest(WebRequest wrappedRequest, bool compressRequest, bool acceptCompressedResponse)
        {
            this.wr = wrappedRequest;
            this.gzipRequest = compressRequest;
            if(this.gzipRequest)
                wr.Headers["Content-Encoding"] = GZIP;
            if(acceptCompressedResponse)
                wr.Headers["Accept-Encoding"] = GZIP;
        }

        private bool gzipRequest;
        private WebRequest wr;

        // most of these just delegate to the contained WebRequest
        public override string Method
        {
            get { return wr.Method; }
            set { wr.Method = value; }
        }
	
        public override Uri RequestUri
        {
            get { return wr.RequestUri; }
        }
	
        public override WebHeaderCollection Headers
        {
            get { return wr.Headers; }
            set { wr.Headers = value; }
        }
	
        public override long ContentLength
        {
            get { return wr.ContentLength; }
            set { wr.ContentLength = value; }
        }
	
        public override string ContentType
        {
            get { return wr.ContentType; }
            set { wr.ContentType = value; }
        }
	
        public override ICredentials Credentials
        {
            get { return wr.Credentials; }
            set { wr.Credentials = value; }
        }
	
        public override bool PreAuthenticate
        {
            get { return wr.PreAuthenticate; }
            set { wr.PreAuthenticate = value; }
        }
	
        private Stream request_stream = null;

        public override System.IO.Stream GetRequestStream()
        {
            return WrappedRequestStream(wr.GetRequestStream());
        }
	
        public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
        {
            return wr.BeginGetRequestStream (callback, state);
        }
	
        public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
        {
            return WrappedRequestStream(wr.EndGetRequestStream (asyncResult));
        }
	
        /// <summary>
        /// helper function that wraps the request stream in a GzipOutputStream, if we're going to be compressing the request
        /// </summary>
        /// <param name="requestStream"></param>
        /// <returns></returns>
        private Stream WrappedRequestStream(Stream requestStream)
        {
            if ( request_stream == null )
            {
                request_stream = requestStream;
                if(this.gzipRequest)
                    request_stream = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(request_stream);
            }
            return request_stream;
        }

        public override WebResponse GetResponse()
        {
            return new GzipWebResponse(wr.GetResponse ());
        }
	
        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return wr.BeginGetResponse (callback, state);
        }
	
        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return new GzipWebResponse(wr.EndGetResponse (asyncResult));
        }
    }

    /// <summary>
    /// This is an implementation of WebResponse that delegates to another WebResponse implementation.
    /// It will automatically insert a GzipInputStream into the ResponseStream, if the response
    /// indicates that its gzip compressed.
    /// </summary>
    public class GzipWebResponse : WebResponse	
    {
        private WebResponse wr;
        private Stream response_stream = null;

        internal GzipWebResponse(WebResponse wrapped) 
        {
            this.wr = wrapped;
        }

        /// <summary>
        /// Wrap the returned stream in a gzip uncompressor if needed
        /// </summary>
        /// <returns></returns>
        public override Stream GetResponseStream()
        {
            if ( response_stream == null )
            {
                response_stream = wr.GetResponseStream();
                if ( string.Compare(Headers["Content-Encoding"], "gzip", true) == 0 )
                    response_stream = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(response_stream);
            }
            return response_stream;
        }

        // these all delegate to the contained WebResponse
        public override long ContentLength
        {
            get { return wr.ContentLength; }
            set { wr.ContentLength = value; }
        }
	
        public override string ContentType
        {
            get { return wr.ContentType; }
            set { wr.ContentType = value; }
        }
	
        public override Uri ResponseUri
        {
            get { return wr.ResponseUri; }
        }
	
        public override WebHeaderCollection Headers
        {
            get { return wr.Headers; }
        }
    }
}