using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Yebob.Utils
{
    public class UriTemplate
    {
        private static Regex VARIABLENAMES_REGEX = new Regex(@"\{([^/]+?)\}");
	    private static string VARIABLEVALUE_PATTERN = "(?<{0}>.*)";
        
        private const string BRACE_LEFT = "{";
        private const string BRACE_RIGHT = "}";

        private string uriTemplate;
        private string[] variableNames;
        private Regex matchRegex;

        public string[] VariableNames
        {
            get { return this.variableNames; }
        }

        public UriTemplate(string uriTemplate)
        {
            this.uriTemplate = uriTemplate;
            Parser parser = new Parser(uriTemplate);
            this.variableNames = parser.GetVariableNames();
            this.matchRegex = parser.GetMatchRegex();
        }

        public Uri Expand(IDictionary<string, object> uriVariables)
        {
            if (uriVariables.Count != this.variableNames.Length)
            {
                throw new ArgumentException(String.Format(
                        "Invalid amount of variables values in '{0}': expected {1}; got {2}",
                        this.uriTemplate, this.variableNames.Length, uriVariables.Count));
            }

            string uri = this.uriTemplate;
            foreach (string variableName in this.variableNames)
            {
                if (!uriVariables.ContainsKey(variableName))
                {
                    throw new ArgumentException(String.Format(
                        "'uriVariables' dictionary has no value for '{0}'",
                        variableName));
                }
                uri = Replace(uri, variableName, uriVariables[variableName]);
            }

            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        public Uri Expand(params object[] uriVariableValues)
        {
            if (uriVariableValues.Length != this.variableNames.Length)
            {
                throw new ArgumentException(String.Format(
                        "Invalid amount of variables values in '{0}': expected {1}; got {2}",
                        this.uriTemplate, this.variableNames.Length, uriVariableValues.Length));
            }

            string uri = this.uriTemplate;
            for (int i = 0; i < this.variableNames.Length; i++)
            {
                uri = Replace(uri, this.variableNames[i], uriVariableValues[i]);
            }

            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        public bool Matches(string uri)
        {
            if (uri == null)
            {
                return false;
            }
            return this.matchRegex.IsMatch(uri);
        }

        public IDictionary<string, string> Match(string uri)
        {
            ArgumentUtils.AssertNotNull(uri, "uri");

            IDictionary<string, string> result = new Dictionary<string, string>();
            Match match = this.matchRegex.Match(uri);
            for (int i = 1; i < match.Groups.Count; i++ )
            {
                result.Add(this.matchRegex.GroupNameFromNumber(i), match.Groups[i].Value);
            }
            return result;
        }

        public override string ToString()
        {
            return this.uriTemplate;
        }

        private static string Replace(string uriTemplate, string token, object value)
        {
            string quotedToken = BRACE_LEFT + token + BRACE_RIGHT;
            return uriTemplate.Replace(quotedToken, (value == null) ? String.Empty : value.ToString());
        }	    
        
        private class Parser 
        {
		    private List<String> variableNames = new List<String>();
		    private StringBuilder patternBuilder = new StringBuilder();

		    public Parser(string uriTemplate) 
            {
			    ArgumentUtils.AssertNotNull(uriTemplate, "uriTemplate");

                int index = 0;
                this.patternBuilder.Append("^");
                foreach (Match match in VARIABLENAMES_REGEX.Matches(uriTemplate))
                {
                    string variableName = match.Groups[1].Value;
                    if (!variableNames.Contains(variableName))
                    {
                        variableNames.Add(variableName);
                    }

                    this.patternBuilder.Append(Escape(uriTemplate, index, match.Index - index));
                    this.patternBuilder.Append(String.Format(VARIABLEVALUE_PATTERN, variableName));
                    index = match.Index + match.Length;
                }
                this.patternBuilder.Append(Escape(uriTemplate, index, uriTemplate.Length - index));
                this.patternBuilder.Append("$");
		    }

            private static string Escape(String fullPath, int start, int end)
            {
                if (start == end)
                {
                    return "";
                }
                return Regex.Escape(fullPath.Substring(start, end));
            }

            public string[] GetVariableNames() 
            {
			    return this.variableNames.ToArray();
		    }

            public Regex GetMatchRegex() 
            {
                return new Regex(this.patternBuilder.ToString());
		    }
	    }
    }
}
