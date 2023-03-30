using CsGOStateEmitter.Helper;
using System.Reflection;

namespace CsGOStateEmitter
{
    public class RankService
    {

        public static string GetFileLayout(string path, string nameFile)
        {
            //var rootPath = ConfigHelper.Get<string>($"Pathwwwroot");
            //if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(nameFile) || string.IsNullOrWhiteSpace(path))
            //    return string.Empty;

            //string applicationPath = Path.Combine(Directory.GetCurrentDirectory(), @$"{rootPath}");

            var pathCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if DEBUG
            pathCurrentDirectory = Directory.GetCurrentDirectory();
#endif

            return Path.Combine(Path.Combine(pathCurrentDirectory, path), nameFile);
        }

        public static string GetRank(int points)
        {
            if (points > 0 && points <= 500)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silver1.jpeg";
            if (points > 500)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silver2.jpeg";

            if (points > 1100)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silver3.jpeg";

            if (points > 1700)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silver4.jpeg";

            if (points > 2300)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silverElite.jpeg";

            if (points > 2800)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/silverEliteMaster.jpeg";

            if (points > 3200)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/gold.jpeg";

            if (points > 3700)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/gold2.jpeg";

            if (points > 4300)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/gold3.jpeg";

            if (points > 4900)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/gold4.jpeg";

            if (points > 5200)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/ak1.jpeg";

            if (points > 5700)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/ak2.jpeg";

            if (points > 6300)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/akx.jpeg";

            if (points > 6800)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/xerif.jpeg";

            if (points > 7300)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/eagle1.jpeg";

            if (points > 7800)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/eagle2.jpeg";

            if (points > 8300)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/supreme.jpeg";

            if (points > 10000)
                return "http://mixcsgo.servegame.com:27016/SkinkiDriver/get-file/levels/levels/levels/global.jpeg";

            return "";
        }
    }
}
