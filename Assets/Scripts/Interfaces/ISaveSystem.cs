namespace SurvivalShooter.SaveSystem
{
    
    public interface ISaveSystem
    {
        bool SaveExists { get; }
        void Save(SaveData data);
        SaveData Load();   
        void Delete();
    }
}