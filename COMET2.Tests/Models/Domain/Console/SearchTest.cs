using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COMET.Tests.Models.Domain.Console {
    [TestClass]
    public class SearchTest {
        private string text = "The quick red fox jumped over the lazy brown dog";
        
        [TestMethod]
        public void TestMiddle() {
            string search = "red fox";

            System.Diagnostics.Debug.WriteLine(returnWrap(text, search));
        }

        [TestMethod]
        public void TestEnd() {
            string search = "dog";
            
            System.Diagnostics.Debug.WriteLine(returnWrap(text, search));
        }

        private string returnWrap(string text, string search) {
            int charLimit = 20;
            string returnText = text;
            text = text.ToLower();
            search = search.ToLower();

            int start, end;
            start = text.IndexOf(search);
            end = start + search.Length;

            bool leftOk, rightOk;
            leftOk = rightOk = false;
            while (!(leftOk && rightOk)) {
                if (!leftOk) {
                    if (charLimit - (end - start) > 0 && start > 0)
                        start--;
                    else
                        leftOk = true;
                }
                if (!rightOk) {
                    if (charLimit - (end - start) > 0 && end < text.Length - 1)
                        end++;
                    else
                        rightOk = true;
                }
            }

            text = returnText = (start > 0 ? "..." : "") + returnText.Substring(start, end - start) + (end <= text.Length - 1 ? "..." : "");
            text = text.ToLower();
            start = text.IndexOf(search);
            end = start + search.Length;
            returnText = returnText.Replace(returnText.Substring(start, end - start), "<mark>" + returnText.Substring(start, end - start) + "</mark>");
            return returnText;
        }
    }
}
