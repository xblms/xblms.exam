namespace XBLMS.Services
{
    public partial interface IPathManager
    {
        void AddWaterMarkForCertificateReviewAsync(string imagePath, string text, int fontSize, int x, int y);
        void AddImageMarkForCertificateReviewAsync(string imagePath, string waterMarkImagePath, int x, int y, int width, int height);
    }
}
