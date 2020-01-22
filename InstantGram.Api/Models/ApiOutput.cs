namespace InstantGram.Api.Models
{
    public class ApiOutput<T> where T : class
    {
        public int ResponseStatus { get; set; }

        public T Result { get; set; }

        public object Error { get; set; }

    }
}