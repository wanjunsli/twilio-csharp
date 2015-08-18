using System;
using RestSharp;
using RestSharp.Extensions;

namespace Twilio
{
	public partial class TwilioRestClient
	{
		/// <summary>
		/// Retrieve the account details for the currently authenticated account
		/// </summary>
		/// <param name="callback">Method to call upon successful completion</param>
		public virtual void GetAccount(Action<Account> callback)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}.json";
			
			ExecuteAsync<Account>(request, (response) => { callback(response); });
		}

		/// <summary>
		/// Retrieve the account details for a subaccount
		/// </summary>
		/// <param name="accountSid">The Sid of the subaccount to retrieve</param>
		/// <param name="callback">Method to call upon successful completion</param>
        public virtual void GetAccount(string accountSid, Action<Account> callback)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}.json";
			
			request.AddUrlSegment("AccountSid", accountSid);

			ExecuteAsync<Account>(request, (response) => { callback(response); });
		}

		/// <summary>
		/// List all subaccounts created for the authenticated account
		/// </summary>
		/// <param name="callback">Method to call upon successful completion</param>
        public virtual void ListSubAccounts(Action<AccountResult> callback)
		{
            ListSubAccounts(String.Empty, callback);
		}

        /// <summary>
        /// List all subaccounts created for the authenticated account. Makes a GET request to the Account List resource.
        /// </summary>
        /// <param name="friendlyName">Only return the Account resources with friendly names that exactly match this name</param>
        public virtual void ListSubAccounts(string friendlyName, Action<AccountResult> callback)
        {
            ListSubAccounts(friendlyName, String.Empty, callback);
        }

        /// <summary>
        /// List all subaccounts created for the authenticated account. Makes a GET request to the Account List resource.
        /// </summary>
        /// <param name="friendlyName">Only return the Account resources with friendly names that exactly match this name</param>
        /// <param name="status">Only return Account resources with the given status. Can be closed, suspended or active</param>
        public virtual void ListSubAccounts(string friendlyName, string status, Action<AccountResult> callback)
        {
            ListSubAccounts(friendlyName, status, 50, callback);
        }

        /// <summary>
        /// List all subaccounts created for the authenticated account. Makes a GET request to the Account List resource.
        /// </summary>
        public virtual void ListSubAccounts(int count, Action<AccountResult> callback)
        {
            ListSubAccounts(String.Empty, count, callback);
        }

        /// <summary>
        /// List subaccounts that match the provided FriendlyName for the authenticated account. Makes a GET request to the Account List resource.
        /// </summary>
        /// <param name="friendlyName">Only return the Account resources with friendly names that exactly match this name</param>
        public virtual void ListSubAccounts(string friendlyName, int count, Action<AccountResult> callback)
        {
            ListSubAccounts(friendlyName, string.Empty, count, callback);
        }

        /// <summary>
        /// List subaccounts that match the provided FriendlyName for the authenticated account. Makes a GET request to the Account List resource.
        /// </summary>
        /// <param name="friendlyName">Only return the Account resources with friendly names that exactly match this name</param>
        /// <param name="status">Only return Account resources with the given status. Can be closed, suspended or active</param>
        public virtual void ListSubAccounts(string friendlyName, string status, int count, Action<AccountResult> callback)
        {
            var request = new RestRequest();
            request.Resource = "Accounts.json";

            if (friendlyName.HasValue()) { request.AddParameter("FriendlyName", friendlyName); }
            if (friendlyName.HasValue()) { request.AddParameter("Status", status); }

            // Paging options
            request.AddParameter("PageSize", count);

            ExecuteAsync<AccountResult>(request, (response) => { callback(response); });
        }

		/// <summary>
		/// Creates a new subaccount under the authenticated account
		/// </summary>
		/// <param name="friendlyName">Name associated with this account for your own reference (can be empty string)</param>
		/// <param name="callback">Method to call upon successful completion</param>
        public virtual void CreateSubAccount(string friendlyName, Action<Account> callback)
		{
			var request = new RestRequest(Method.POST);
			request.Resource = "Accounts.json";
			
			request.AddParameter("FriendlyName", friendlyName);

			ExecuteAsync<Account>(request, (response) => { callback(response); });
		}

		/// <summary>
		/// Changes the status of a subaccount. You must be authenticated as the master account to call this method on a subaccount.
		/// WARNING: When closing an account, Twilio will release all phone numbers assigned to it and shut it down completely. 
		/// You can't ever use a closed account to make and receive phone calls or send and receive SMS messages. 
		/// It's closed, gone, kaput. It will still appear in your accounts list, and you will still have access to historical 
		/// data for that subaccount, but you cannot reopen a closed account.
		/// /// </summary>
		/// <param name="subAccountSid">The subaccount to change the status on</param>
		/// <param name="status">The status to change the subaccount to</param>
		/// <param name="callback">Method to call upon successful completion</param>
        public virtual void ChangeSubAccountStatus(string subAccountSid, AccountStatus status, Action<Account> callback)
		{
			if (subAccountSid == AccountSid)
			{
				throw new InvalidOperationException("Subaccount status can only be changed when authenticated from the master account.");
			}

			var request = new RestRequest(Method.POST);
			request.Resource = "Accounts/{AccountSid}.json";
			
			request.AddParameter("Status", status.ToString().ToLower());
			request.AddUrlSegment("AccountSid", subAccountSid);

			ExecuteAsync<Account>(request, (response) => { callback(response); });
		}

		/// <summary>
		/// Update the friendly name associated with the currently authenticated account
		/// </summary>
		/// <param name="friendlyName">Name to use when updating</param>
		/// <param name="callback">Method to call upon successful completion</param>
        public virtual void UpdateAccountName(string friendlyName, Action<Account> callback)
		{
			var request = new RestRequest(Method.POST);
			request.Resource = "Accounts/{AccountSid}.json";
						request.AddParameter("FriendlyName", friendlyName);

			ExecuteAsync<Account>(request, (response) => { callback(response); });
		}
	}
}