namespace Sweeper.ViewModels.Interfaces
{
    public interface IAdornGameWithSounds
    {
        void ClickItemOK();
        void Lost();
        void NewGame();
        void Won();
        void NewRecord();
        void Swoosh();
        void Dispose();
    }
}
