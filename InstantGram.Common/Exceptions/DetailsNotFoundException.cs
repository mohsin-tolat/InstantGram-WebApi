using System;

namespace InstantGram.Common.Exceptions
{
    [Serializable]
    public class DetailsNotFoundException : Exception
    {
        public DetailsNotFoundException(int id)
            : base(String.Format("Details Not Found for Id: {id}", id))
        {

        }

        public DetailsNotFoundException(string errorMessage)
            : base(errorMessage)
        {

        }

        public DetailsNotFoundException(string parameterName, object parameterValue)
            : base(String.Format("Details Not Found for Parameter: {0} and Value: {1}", parameterName, parameterValue))
        {

        }

    }
}