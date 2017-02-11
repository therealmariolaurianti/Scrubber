namespace Scrubber.Interfaces
{
    public interface IOptions
    {
        bool ClearComments { get; set; }
        string FolderPath { get; set; }
    }
}