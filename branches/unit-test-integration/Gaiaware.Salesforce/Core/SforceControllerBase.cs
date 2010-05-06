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
using System.Collections.Generic;

namespace Gaiaware.Salesforce
{
    public abstract class SforceControllerBase
    {
        protected IEnumerable<T> ConvertQueryResult<T>(QueryResult result) where T : sObject
        {
            var data = new T[result.size];
            for (int i = 0; i < data.Length; i++)
            {
                yield return (T) result.records[i];
            }
        }

        protected IEnumerable<string> ParseSaveResult(SaveResult[] saveResults, out List<string> errors)
        {
            errors = null;
            if (saveResults == null)
                return null;

            var retVal = new List<string>();
            if (saveResults.Length > 0)
            {
                for (int i = 0; i < saveResults.Length; i++)
                    //add to return value if success
                    if (saveResults[i].success)
                        retVal.Add(saveResults[i].id);
                    else
                    {
                        if (errors == null) errors = new List<string>();
                        errors.Add(GetErrorMessage(saveResults[i].errors));
                    }
            }

            return retVal;
        }

        private static string GetErrorMessage(IEnumerable<Error> errors)
        {
            string errorMsg = "";
            foreach (Error err in errors)
                errorMsg += string.Format("Code:{0} - Message:{1}", err.statusCode, err.message);

            return errorMsg;
        }

        protected string ParseFirstSaveResult(params SaveResult[] saveResults)
        {
            if (saveResults == null)
                return string.Empty;

            if ((saveResults.Length == 1) && (saveResults[0].success))
            {
                return saveResults[0].id;
            }

            // fetch all errors and rethrow as exception
            string errors = String.Empty;
            foreach (SaveResult result in saveResults)
                foreach (Error err in result.errors)
                    errors += string.Format("Code:{0} - Message:{1}", err.statusCode, err.message);

            // TODO: find a way to persist the errors to easen support

            if (!String.IsNullOrEmpty(errors))
                throw new Exception(errors);

            // otherwise return empty string
            return string.Empty;
        }
    }
}