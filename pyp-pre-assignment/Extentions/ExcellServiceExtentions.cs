namespace pyp_pre_assignment.Extentions
{
    public static class ExcellServiceExtentions
    {
        public static bool IsExcell(this IFormFile file)
        {
            string fileExt = Path.GetExtension(file.FileName);

            if (fileExt == ".xls" || fileExt == ".xlsx")
            {
                return true;
            }

            return false;
        }

        public static bool ExcelSize(this IFormFile file, int size)
        {
            return file.Length / 1024 > size;
        }
    }
}
