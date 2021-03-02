namespace Cybtans.Graphics.Importers
{
    public interface IAnimationImporter
    {
        void ImportAnimation(Scene scene, string filename);

        void ImportAnimation(Scene scene, string filename, Frame root, string fileRoot);
    }

}
