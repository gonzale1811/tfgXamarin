package crc646f870163436f3bb5;


public class FirebaseConsultService
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.tasks.OnSuccessListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSuccess:(Ljava/lang/Object;)V:GetOnSuccess_Ljava_lang_Object_Handler:Android.Gms.Tasks.IOnSuccessListenerInvoker, Xamarin.GooglePlayServices.Tasks\n" +
			"";
		mono.android.Runtime.register ("InspectionManager.Droid.Servicios.FirebaseConsultService, InspectionManager.Android", FirebaseConsultService.class, __md_methods);
	}


	public FirebaseConsultService ()
	{
		super ();
		if (getClass () == FirebaseConsultService.class)
			mono.android.TypeManager.Activate ("InspectionManager.Droid.Servicios.FirebaseConsultService, InspectionManager.Android", "", this, new java.lang.Object[] {  });
	}


	public void onSuccess (java.lang.Object p0)
	{
		n_onSuccess (p0);
	}

	private native void n_onSuccess (java.lang.Object p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
