namespace Bleak.Helpers
{
    public static class Files
    {
        public static Dictionary<int, string> IdMap = new()
        {
            {0, "text" },
            {1, "pdf" },
            {2, "video" },
            {3, "audio" },
            {4, "image" },
            {5, "binary" },
        };

        private static readonly Dictionary<string, int> _matchers = new()
        {
            // 0 = text
            { "txt", 0 }, { "md", 0 }, { "csv", 0 }, { "log", 0 }, { "json", 0 }, { "xml", 0 }, { "html", 0 }, { "htm", 0 },

            // 1 = pdf
            { "pdf", 1 },

            // 2 = video
            { "mp4", 2 }, { "mkv", 2 }, { "avi", 2 }, { "mov", 2 }, { "wmv", 2 }, { "flv", 2 }, { "webm", 2 }, { "mpeg", 2 }, { "mpg", 2 },

            // 3 = audio
            { "mp3", 3 }, { "wav", 3 }, { "aac", 3 }, { "flac", 3 }, { "ogg", 3 }, { "wma", 3 }, { "m4a", 3 },

            // example: 4 = image
            { "jpg", 4 }, { "jpeg", 4 }, { "png", 4 }, { "gif", 4 }, { "bmp", 4 }, { "tiff", 4 }, { "webp", 4 }, { "svg", 4 },

            // 5 = unknown (fallback)
            { "bin", 5 }, { "exe", 5 }, { "dll", 5 }, { "dat", 5 }, { "", 5 },
        };

        public static int GetFileType(IFormFile file)
        {
            var extension = file.FileName.Split(".").Last().ToLower().Trim();
            var matched = _matchers.Keys.ToList().Find(ext => ext == extension);
            return matched is null ? 5 : _matchers[matched];
        }
    }
}
