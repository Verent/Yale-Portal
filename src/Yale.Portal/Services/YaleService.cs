using System;
using System.Diagnostics;
using System.Linq;
using Yale.Engine;

namespace Yale.Portal.Services
{
    public class YaleService : ComputeInstance
    {
        public int MaxExpressions = 5;
        public int MaxVariables = 5;

        public YaleService()
        {
            Imports.AddType(typeof(Math));
        }

        /// <summary>
        /// Return empty string if expression does not exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string TryEvaluate(string expression)
        {
            return ContainsExpression(expression)
                ? GetResult(expression)?.ToString() ?? string.Empty
                : string.Empty;
        }

        /// <summary>
        /// Silences exceptions and ensures max expressions in instance
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool AddExpression(string input)
        {
            try
            {
                var values = input.Split(':');

                if (ExpressionKeys.Count() > MaxExpressions && ExpressionKeys.Any(i => i.Equals(values[0])) == false) return false;
                SetExpression(values[0], values[1]);

                return true;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// Ensures max variables in instance
        /// </summary>
        /// <param name="input"></param>
        public void AddValue(string input)
        {
            var values = input.Split('=');
            var key = values[0];
            var value = values[1];

            if (Variables.Count > MaxVariables && Variables.ContainsKey(key) == false) return;

            if (int.TryParse(value, out var integer))
            {
                Variables[key] = integer;
            }
            else if (double.TryParse(value, out var number))
            {
                Variables[key] = number;
            }
            else if (bool.TryParse(value, out var boolean))
            {
                Variables[key] = boolean;
            }
            else
            {
                Variables[key] = value.ToString();
            }
        }
    }
}