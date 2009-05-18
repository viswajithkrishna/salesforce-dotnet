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
using System.Collections.Generic;
using Gaiaware.Salesforce;
using Gaiaware.SalesforceWrapper.Utilities;

namespace Gaiaware.SalesforceWrapper.Controllers
{
    public class AccountController : SforceControllerBase
    {
        public Account GetWithSubData(string accountId)
        {
            string sql = string.Format(
                @"SELECT {0}, (select {1} from contracts), (select {2} from contacts) FROM Account where Id='{3}'",
                SforceFields.AccountFields,
                SforceFields.ContractFields,
                SforceFields.ContactFields,
                accountId);

            return SforceProvider.Instance.Get<Account>(sql);

        }

        public IEnumerable<Account> List()
        {
            return
                SforceProvider.Instance.List<Account>(
                    "SELECT {0} FROM Account where name <> ''", SforceFields.AccountFields);

        }

        public Account Get(string accountId)
        {
            return
                SforceProvider.Instance.Get<Account>(
                    "SELECT {0} FROM Account where Id='{1}'", SforceFields.AccountFields, accountId);

        }

        public Account GetByName(string accountName)
        {
            return
                SforceProvider.Instance.Get<Account>(
                    "SELECT {0} from Account where Name='{1}'", SforceFields.AccountFields, accountName);
            
        }

        public string Update(Account account)
        {
            return ParseFirstSaveResult(SforceProvider.Instance.Update(account));
        }
    }
}