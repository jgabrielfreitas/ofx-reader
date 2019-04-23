using System;

namespace OFX.Reader.Infrastructure.Extensions {

    public static class StringExtensions {

        public static string ReadTagName(this string line) {
            
            int index = line.IndexOf("<", StringComparison.Ordinal) + 1;
            int endPosition = line.IndexOf(">", StringComparison.Ordinal);
            
            endPosition = endPosition - index;
            return line.Substring(index, endPosition);
        }

        public static string ReadTagValue(this string line) {
            
            int index = line.IndexOf(">", StringComparison.Ordinal) + 1;
            string value = line.Substring(index).Trim();
            
            if (value.IndexOf("[", StringComparison.Ordinal) != -1) 
                value = value.Substring(0, 8);

            return value;
        }
    }

}