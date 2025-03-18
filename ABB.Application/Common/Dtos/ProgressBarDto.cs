namespace ABB.Application.Common.Dtos
{
    public class ProgressBarDto
    {
        public int Remaining { get; set; }

        public int Total { get; set; }

        public void ResetProgress()
        {
            Remaining = 0;
            Total = 0;
        }
    }
}
