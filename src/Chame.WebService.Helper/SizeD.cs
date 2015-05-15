namespace Chame.WebService.Helper
{
    public struct SizeD
    {
        public double Width;
        public double Height;
        public bool IsEmpty;
        public SizeD(double w, double h)
        {
            this.Width = w;
            this.Height = h;
            this.IsEmpty = false;
        }
    }
}