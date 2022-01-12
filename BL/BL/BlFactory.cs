namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBl()
        {
            return BL.BL.Instance;
        }
    }
}
