//
// SecSign ID Api ASP.NET / C#
//
// (c) 2014, 2015 SecSign Technologies Inc.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Reflection;

[assembly: AssemblyTitle("SecSign ID API .NET")]
[assembly: AssemblyProduct("SecSign ID API .NET")]
[assembly: AssemblyCompany("SecSign Technology Inc.")]
[assembly: AssemblyCopyright("2015 SecSign Technology Inc.")]

[assembly: AssemblyVersion("1.1.1")]
[assembly: CLSCompliant(true)]
//[assembly: AssemblyKeyFile(@"SecSignIDApiKey.snk")]

namespace SecSignID
{
	/// <summary>
	/// Class to connect to a SecSign ID server and request authentication sessions and status.
	/// </summary>
	public sealed class SecSignIDApi
	{
		/// <summary>
		/// The secsign id server.
		/// </summary>
		private string secSignIDServer = "https://httpapi.secsign.com";
		
		/// <summary>
		/// The secsign id server port.
		/// </summary>
		private int secSignIDServerPort = 443;
		
		/// <summary>
		/// a fallback secsign id server
		/// </summary>
		private string secSignIDServerFallback = "https://httpapi2.secsign.com";
		
		/// <summary>
		/// port of the fallback secsign id server.
		/// </summary>
		private int secSignIDServerPortFallback = 443;
		
		/// <summary>
		/// The referer which is send to server.
		/// </summary>
		private string referer = "SecSignIDApi_.NET";
		
		/// <summary>
		/// an optional plugin name.
		/// </summary>
		private string pluginName;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecSignID.SecSignIDApi"/> class.
		/// </summary>
		public SecSignIDApi()
		{}
		
		/// <summary>
		/// Sets an optional and additional name of the plugin.
		/// </summary>
		/// <param name='pluginName'>
		/// Plugin name.
		/// </param>
		public void SetPluginName(string pluginName){
			this.pluginName = pluginName;
		}
		
		/// <summary>
		/// Fills the parameter list with all mandatory parameter and their values.
		/// </summary>
		/// <param name='parameterList'>
		/// Parameter list.
		/// </param>
		private void fillParameterArray(Dictionary<string, string> parameterList){
			parameterList.Add("apimethod", this.referer);
			// parameterList.Add("scriptversion", this.version);
		}
		
		/// <summary>
		/// Requests the authsession to login.
		/// </summary>
		/// <returns>
		/// The authsession to login.
		/// </returns>
		/// <param name='secsignid'>the secsign id the auth session shall be created for.</param>
		/// <param name='serviceName'>Service name which is displayed at users smartphone.</param>
		/// <param name='serviceAddress'>Service address which is displayed at users smartphone.</param>
		public AuthSession RequestAuthSession(string secsignid, string serviceName, string serviceAddress)
		{
			if(secsignid == null){
				throw new System.ArgumentException("The SecSign ID must not be null.");
			}
			if(serviceName == null){
				throw new System.ArgumentException("The service name must not be null.");
			}
			if(serviceAddress == null){
				throw new System.ArgumentException("The service address must not be null.");
			}
			
			AuthSession authSession = null;
			Dictionary<string, string> parameterList = new Dictionary<string, string>();
			
			fillParameterArray(parameterList);
			
			parameterList.Add("request", "ReqRequestAuthSession");
			parameterList.Add("secsignid", secsignid);
			parameterList.Add("servicename", serviceName);
			parameterList.Add("serviceaddress", serviceAddress);
			
			if(this.pluginName != null){
				parameterList.Add("pluginname", this.pluginName);
			}
			
			string response = this.SendRequest(parameterList);
			Dictionary<string, string> responseDict = this.CheckResponse(response);
			if(responseDict != null)
			{
				// string secSignID, string authSessionID, string requestID, string requestName, string requestAddress
				authSession = new AuthSession(responseDict["secsignid"], 
					responseDict["authsessionid"], 
					responseDict["requestid"],
					responseDict["servicename"], 
					responseDict["serviceaddress"],
					responseDict["authsessionicondata"]);
			}
		
			return authSession;
		}
		
		/// <summary>
		/// Gets the authentication session state for a certain secsign id whether the authentication session is still pending or it was accepted or denied.
		/// </summary>
		/// <returns>The auth session state.</returns>
		/// <param name='authSession'>AuthSession.</param>
		/// <exception cref='System.ArgumentException'>
		/// Is thrown when an argument passed to a method is invalid.
		/// </exception>
		public int GetAuthSessionState(AuthSession authSession)
		{
			if(authSession == null){
				throw new System.ArgumentException("The auth session must not be null.");
			}
			
			Dictionary<string, string> parameterList = new Dictionary<string, string>();

			fillParameterArray(parameterList);

			parameterList.Add("request", "ReqGetAuthSessionState");
			parameterList.Add("secsignid", authSession.GetSecSignID());
			parameterList.Add("authsessionid", authSession.GetAuthSessionID());
			parameterList.Add("requestid", authSession.GetRequestID());


			int authSessionState = AuthSession.NOSTATE;

			string response = this.SendRequest(parameterList);
			Dictionary<string, string> responseDict = this.CheckResponse(response);
			if(responseDict != null)
			{
				if(responseDict.ContainsKey("authsessionstate"))
				{
					authSessionState = Int32.Parse(responseDict["authsessionstate"]);
				}
			}
		
			return authSessionState;
		}
		
		/// <summary>
		/// Cancel the given auth session
		/// </summary>
		/// <returns>
		/// the auth session state after it has been canceled.
		/// </returns>
		/// <param name='authSession'>AuthSession.</param>
		public int CancelAuthSession(AuthSession authSession)
		{
			if(authSession == null){
				throw new System.ArgumentException("The auth session must not be null.");
			}
			
			Dictionary<string, string> parameterList = new Dictionary<string, string>();

			fillParameterArray(parameterList);

			parameterList.Add("request", "ReqCancelAuthSession");
			parameterList.Add("secsignid", authSession.GetSecSignID());
			parameterList.Add("authsessionid", authSession.GetAuthSessionID());
			parameterList.Add("requestid", authSession.GetRequestID());

			int authSessionState = AuthSession.NOSTATE;

			string response = this.SendRequest(parameterList);
			Dictionary<string, string> responseDict = this.CheckResponse(response);
			if(responseDict != null)
			{
				if(responseDict.ContainsKey("authsessionstate"))
				{
					authSessionState = Int32.Parse(responseDict["authsessionstate"]);
				}
			}
		
			return authSessionState;
		}
		
		/// <summary>
		/// Releases an authentication session if it was accepted and not used any longer
		/// </summary>
		/// <returns>
		/// the auth session state after it has been released.
		/// </returns>
		/// <param name='authSession'>AuthSession.</param>
		public int ReleaseAuthSession(AuthSession authSession)
		{
			if(authSession == null){
				throw new System.ArgumentException("The auth session must not be null.");
			}
			
			Dictionary<string, string> parameterList = new Dictionary<string, string>();

			fillParameterArray(parameterList);

			parameterList.Add("request", "ReqReleaseAuthSession");
			parameterList.Add("secsignid", authSession.GetSecSignID());
			parameterList.Add("authsessionid", authSession.GetAuthSessionID());
			parameterList.Add("requestid", authSession.GetRequestID());

			int authSessionState = AuthSession.NOSTATE;

			string response = this.SendRequest(parameterList);
			Dictionary<string, string> responseDict = this.CheckResponse(response);
			if(responseDict != null)
			{
				if(responseDict.ContainsKey("authsessionstate"))
				{
					authSessionState = Int32.Parse(responseDict["authsessionstate"]);
				}
			}
		
			return authSessionState;
		}
		
		/// <summary>
		/// Actually a request is build and send to server.
		/// </summary>
		/// <returns>response of the server.</returns>
		/// <param name='parameterList'>parameter list which will be url encoded.</param>
		private string SendRequest(Dictionary<string, string> parameterList)
		{
			string parameterString = "";
			foreach(KeyValuePair<string, string> p in parameterList)
			{
				parameterString = parameterString + p.Key + "=" + p.Value + "&";
			}
			byte[] requestData = System.Text.Encoding.UTF8.GetBytes(parameterString);
			
			WebRequest request = null;
			
			try
			{
				request = WebRequest.Create(this.secSignIDServer + ":" + this.secSignIDServerPort);
			}
			catch(System.Net.WebException ex)
			{
				// try fallback server
				if(this.secSignIDServerFallback != null && this.secSignIDServerPortFallback > 0)
				{
					request = WebRequest.Create(this.secSignIDServerFallback + ":" + this.secSignIDServerPortFallback);
				}
				else
				{
					throw ex;
				}
			}
			
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = requestData.Length;
			
			// send data
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(requestData, 0, requestData.Length);
			requestStream.Close();
			
			// get response
			WebResponse response = request.GetResponse();
			
			// get data from response
			Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            
            string responseString = reader.ReadToEnd();

            reader.Close();
            responseStream.Close();
            response.Close();
			
			return responseString;
		}
		
		/// <summary>
		/// Checks the response string.
		/// </summary>
		/// 
		/// <returns>The response.</returns>
		/// <param name='response'>Response.</param>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
		private Dictionary<string, string> CheckResponse(string response)
		{
			if(response == null)
			{
				return null;
			}
			
			Dictionary<string, string> responseDict = new Dictionary<string, string>();
			string[] responseValues = response.Split('&'); 
			foreach(string value in responseValues)
			{
				if(value != null && !value.Equals(""))
				{
					string[] valuePair = value.Split('=');
					responseDict.Add(valuePair[0], valuePair[1]);
				}
			}
				
			// check values?
			if(responseDict.ContainsKey("error"))
			{
				throw new System.Net.WebException(responseDict["error"] + ": " + responseDict["errormsg"]);
			}
			return responseDict;
		}
	}
	
	/// <summary>
	/// Class which represents a mobile authentication session.
	/// </summary>
	public sealed class AuthSession
    {
		/// <summary>
		/// Constant NOSTATE. No State: Used when the session state is undefined. 
		/// </summary>
        public const int NOSTATE = 0;
        
        /// <summary>
        /// Constant PENDING. Pending: The session is still pending for authentication.
        /// </summary>
        public const int PENDING = 1;
        
        /// <summary>
        /// Constant EXPIRED. Expired: The authentication timeout has been exceeded.
        /// </summary>
        public const int EXPIRED = 2;
        
        /// <summary>
        /// Constant ACCEPTED. Accepted: The user was successfully authenticated.
        /// </summary>
        public const int ACCEPTED = 3;
        
        /// <summary>
        /// Constant DENIED. Denied: The user denied this session.
        /// </summary>
        public const int DENIED = 4;
		
        /// <summary>
		/// Constant SUSPENDED. Suspended: The server suspended this session for security reasons.
        /// </summary>
        public const int SUSPENDED = 5;
        
        /// <summary>
		/// Constant CANCELED. Canceled: The service has canceled or withdrawn this session.
        /// </summary>
		public const int CANCELED = 6;
        
        /// <summary>
        /// Constant FETCHED. This session was accepted and then fetched by the service. It can't be used anymore.
        /// </summary>
        public const int FETCHED = 7;
		
		/// <summary>
        /// Constant FETCHED. This session has become invalid.
        /// </summary>
        public const int INVALID = 8;
		
		
		
		 /// <summary>
		 /// the secsign id the session has been craeted for
		 /// </summary>
        private string secSignID;
        
        /// <summary>
        /// the session ID
        /// </summary>
        private string authSessionID;
        
        /// <summary>
        /// the name of the requesting service.
        /// </summary>
        private string requestingService;
		
		/// <summary>
		/// Tthe address, a valid url, of the requesting service. this will be shown at the smartphone
		/// </summary>
		private string requestingServiceAddress;
        
        /// <summary>
        /// the request ID is similar to a session ID. 
        /// it is generated after a authentication session has been created. all other request like dispose, withdraw or to get the auth session state
        /// will be rejected if a request id is not specified.
        /// </summary>
        private string requestID;
		
		/// <summary>
		/// icon data of the so called access pass. the image data needs to be displayed otherwise the user does not know which access apss he needs to choose in order to accept the authentication session.
		/// </summary>
		private string authSessionIconData;

		
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthSession"/> class.
		/// </summary>
		/// <param name='secSignID'>the secsign id</param>
		/// <param name='authSessionID'>the auth session id</param>
		/// <param name='requestID'>the request id sent by the server</param>
		/// <param name='requestName'>the request name</param>
		/// <param name='requestAddress'>the request address or url</param>
		/// <param name='base64EncdodedIconData'>the base64 encoded png data of the access pass</param>
		public AuthSession(string secSignID, string authSessionID, string requestID, string requestName, string requestAddress, string base64EncdodedIconData)
		{
			if(secSignID == null || secSignID.Equals(""))
			{
				throw new System.ArgumentException("The SecSign ID must not be null.");
			}
			if(authSessionID == null || authSessionID.Equals(""))
			{
				throw new System.ArgumentException("The auth session ID must not be null.");
			}
			if(requestID == null || requestID.Equals(""))
			{
				throw new System.ArgumentException("The request ID must not be null.");
			}
			
			this.secSignID = secSignID;
			this.authSessionID = authSessionID;
			this.requestID = requestID;
			
			this.requestingService = requestName;
			this.requestingServiceAddress = requestAddress;
			
			this.authSessionIconData = base64EncdodedIconData;
		}
		
		/// <summary>
		/// Gets the secsign id.
		/// </summary>
		/// <returns>
		/// The sec signer ID.
		/// </returns>
        public string GetSecSignID()
        {
            return this.secSignID;
        }
        
        
        /// <summary>
        /// Gets the authsession ID.
        /// </summary>
        /// <returns>
        /// The authsession ID.
        /// </returns>
        public string GetAuthSessionID()
        {
            return this.authSessionID;
        }
        
        
        /// <summary>
        /// Gets the requesting service.
        /// </summary>
        /// <returns>
        /// The requesting service.
        /// </returns>
        public string GetRequestingService()
        {
            return this.requestingService;
        }
		
		/// <summary>
        /// Gets the requesting service address.
        /// </summary>
        /// <returns>
        /// The requesting service address.
        /// </returns>
        public string GetRequestingServiceAddress()
        {
            return this.requestingServiceAddress;
        }
        
        
        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <returns>
        /// The request ID.
        /// </returns>
        public string GetRequestID()
        {
            return this.requestID;
        }
		
		/// <summary>
		/// Gets the icon data which will be displayed in website.
		/// </summary>
		/// <returns>
		/// The icon data.
		/// </returns>
		public string GetIconData()
        {
            return this.authSessionIconData;
        }
        
        
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="SecSignID.AuthSession"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents the current <see cref="SecSignID.AuthSession"/>.
        /// </returns>
        override public string ToString()
        {
            return this.authSessionID + " (" + this.secSignID + ", " + this.requestingService + ", " + this.requestingServiceAddress + ")";
        }
	}
}

