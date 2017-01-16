using System;
using System.Threading.Tasks;

#if __UNIFIED__
using UIKit;
#else
using MonoTouch.UIKit;
#endif

namespace Auth0.SDK
{
	public class DeviceIdProvider : IDeviceIdProvider
	{
		public Task<string> GetDeviceId ()
		{
			return Task.FromResult<string>(UIDevice.CurrentDevice.Name);
		}
	}
}

