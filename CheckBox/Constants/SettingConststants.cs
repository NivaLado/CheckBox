using Plugin.Media.Abstractions;

namespace CheckBox.Constants
{
	public class SettingConstants
	{
		// Cross media settings
		// public static string Directory;
		public static bool AllowCropping = true;
		public static CameraDevice DefaultCamera = CameraDevice.Rear;
		public static int MaxWidthHeight = 2000;
        public static bool SaveToAlbum = true;
		public static PhotoSize PhotoSize = PhotoSize.Medium;
		public static int CompressionQuality = 80;
		public static bool RotateImage = true;
	}
}