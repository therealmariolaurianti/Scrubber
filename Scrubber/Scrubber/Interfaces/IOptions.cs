namespace Scrubber.Interfaces
{
    public interface IOptions
    {
        string FolderPath { get; set; }
        bool ClearComments { get; set; }
    }
}