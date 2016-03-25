using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigT
{
    public static class Csv
    {
        private class CsvReadState
        {
            public bool InsideQuote { get; set; }
            public List<IList<string>> Values { get; }
            public List<string> CurrentRecord { get; set; }
            public StringBuilder CurrentField { get; }

            public CsvReadState()
            {
                CurrentField = new StringBuilder();
                CurrentRecord = new List<string>();
                Values = new List<IList<string>>();
            }
        }

        public static IList<IList<string>> ParseValues(TextReader reader, char delimiter, char qualifier)
        {
            var state = new CsvReadState();
            while (reader.Peek() != -1)
            {
                var readChar = (char)reader.Read();
                if (IsLineEnd(readChar, reader))
                    ProcessLineEnd(readChar, reader, state);
                else if (IsFieldBeginning(state))
                    ProcessFieldBeginning(readChar, state, qualifier, delimiter);
                else if (readChar == delimiter)
                    ProcessDelimiter(delimiter, state);
                else if (readChar == qualifier)
                    ProcessQualifier(qualifier, reader, state);
                else
                    state.CurrentField.Append(readChar);
            }
            AddCurrentRecordToValues(state);
            return state.Values;
        }

        private static bool IsLineEnd(char readedCharacter, TextReader reader)
        {
            return readedCharacter == '\n' || (readedCharacter == '\r' && (char) reader.Peek() == '\n');
        }

        private static void ProcessLineEnd(char readedCharacter, TextReader reader, CsvReadState state)
        {
            // If it's a \r\n combo consume the \n part and throw it away.
            if (readedCharacter == '\r')
                reader.Read();
            if (state.InsideQuote)
            {
                if (readedCharacter == '\r')
                    state.CurrentField.Append('\r');
                state.CurrentField.Append('\n');
            }
            else
            {
                AddCurrentRecordToValues(state);
                state.CurrentRecord = new List<string>(state.CurrentRecord.Count);
            }
        }

        private static void AddCurrentRecordToValues(CsvReadState state)
        {
            if (state.CurrentRecord.Count > 0 || state.CurrentField.Length > 0)
                AddCurŕentFieldToRecord(state);
            if (state.CurrentRecord.Count > 0)
                state.Values.Add(state.CurrentRecord);
        }

        private static void AddCurŕentFieldToRecord(CsvReadState state)
        {
            state.CurrentRecord.Add(state.CurrentField.ToString());
            state.CurrentField.Clear();
        }

        private static bool IsFieldBeginning(CsvReadState state)
        {
            return state.CurrentField.Length == 0 && !state.InsideQuote;
        }

        private static void ProcessFieldBeginning(char readedCharacter, CsvReadState state, char qualifier, char delimiter)
        {
            if (readedCharacter == qualifier)
                state.InsideQuote = true;
            else if (readedCharacter == delimiter)
                AddCurŕentFieldToRecord(state);
            else if (!char.IsWhiteSpace(readedCharacter)) //Leading whitespace is skipped
                state.CurrentField.Append(readedCharacter);
        }

        private static void ProcessDelimiter(char delimiter, CsvReadState state)
        {
            if (state.InsideQuote)
                state.CurrentField.Append(delimiter);
            else
                AddCurŕentFieldToRecord(state);
        }

        private static void ProcessQualifier(char qualifier, TextReader reader, CsvReadState state)
        {
            if (state.InsideQuote)
            {
                if (IsReaderInEscapedQualifier(reader, qualifier))
                    ProcessEscapedQualifier(reader, state, qualifier);
                else
                    state.InsideQuote = false;
            }
            else
                state.CurrentField.Append(qualifier);
        }

        private static bool IsReaderInEscapedQualifier(TextReader reader, char qualifier)
        {
            return (char) reader.Peek() == qualifier;
        }

        private static void ProcessEscapedQualifier(TextReader reader, CsvReadState state, char qualifier)
        {
            reader.Read();
            state.CurrentField.Append(qualifier);
        }
    }
}
