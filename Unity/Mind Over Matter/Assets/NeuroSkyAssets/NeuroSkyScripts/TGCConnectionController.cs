using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class TGCConnectionController : MonoBehaviour {
	private TcpClient client; 
  	private Stream stream;
  	private byte[] buffer;
	
	public delegate void UpdateIntValueDelegate(int value);
	public delegate void UpdateFloatValueDelegate(float value);
	
	public event UpdateIntValueDelegate UpdatePoorSignalEvent;
	public event UpdateIntValueDelegate UpdateAttentionEvent;
	public event UpdateIntValueDelegate UpdateMeditationEvent;
	public event UpdateIntValueDelegate UpdateRawdataEvent;
	public event UpdateIntValueDelegate UpdateBlinkEvent;
	
	public event UpdateFloatValueDelegate UpdateDeltaEvent;
	public event UpdateFloatValueDelegate UpdateThetaEvent;
	public event UpdateFloatValueDelegate UpdateLowAlphaEvent;
	public event UpdateFloatValueDelegate UpdateHighAlphaEvent;
	public event UpdateFloatValueDelegate UpdateLowBetaEvent;
	public event UpdateFloatValueDelegate UpdateHighBetaEvent;
	public event UpdateFloatValueDelegate UpdateLowGammaEvent;
	public event UpdateFloatValueDelegate UpdateHighGammaEvent;

    private bool connected = false;

    public int signalStrength { get; protected set; }
    public int attention { get; protected set; }
    public int meditation { get; protected set; }

	void Start()
    {
        if(OGInput.instance.useNeurosky)
		    Connect();
	}
	
	public void Disconnect()
    {
        if(connected)
        {
            stream.Close();
            connected = false;
        }
	}
	
	public void Connect()
    {
		if(!connected)
        {
			client = new TcpClient("127.0.0.1", 13854);	
		    stream = client.GetStream();
		    buffer = new byte[1024];
		    byte[] myWriteBuffer = Encoding.ASCII.GetBytes(@"{""enableRawOutput"": true, ""format"": ""Json""}");
		    stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

            connected = true;
		}
	}
	
	void Update()
    {
	    if(connected && stream.CanRead)
        {
	      try { 
	        int bytesRead = stream.Read(buffer, 0, buffer.Length);
	
	        string[] packets = Encoding.ASCII.GetString(buffer, 0, bytesRead).Split('\r');
	
	        foreach(string packet in packets){
	          if(packet.Length == 0)
	            continue;
	
	          IDictionary primary = (IDictionary)JsonConvert.Import(typeof(IDictionary), packet);
	
	          if(primary.Contains("poorSignalLevel"))
              {
                signalStrength = (int.Parse(primary["poorSignalLevel"].ToString()));

				if(UpdatePoorSignalEvent != null)
				   UpdatePoorSignalEvent(int.Parse(primary["poorSignalLevel"].ToString()));
						
	            if(primary.Contains("eSense"))
                {
	              IDictionary eSense = (IDictionary)primary["eSense"];
                  attention = int.Parse(eSense["attention"].ToString());
                  meditation = int.Parse(eSense["meditation"].ToString());
                  if (UpdateAttentionEvent != null){
                     UpdateAttentionEvent(attention);
				   }		
				  if(UpdateMeditationEvent != null){
					 UpdateMeditationEvent(int.Parse(eSense["meditation"].ToString()));
				   }
	            }
	
	            if(primary.Contains("eegPower"))
                {
	                IDictionary eegPowers = (IDictionary)primary["eegPower"];
				    			
				    if(UpdateDeltaEvent != null){
					    UpdateDeltaEvent(float.Parse(eegPowers["delta"].ToString()));			
					}
				  if(UpdateThetaEvent != null){
					 UpdateThetaEvent(float.Parse(eegPowers["theta"].ToString()));			
					}
				  if(UpdateLowAlphaEvent != null){
					 UpdateLowAlphaEvent(float.Parse(eegPowers["lowAlpha"].ToString()));
					}
				  if(UpdateHighAlphaEvent != null){
					 UpdateHighAlphaEvent(float.Parse(eegPowers["highAlpha"].ToString()));
					}
				  if(UpdateLowBetaEvent != null){
					 UpdateLowBetaEvent(float.Parse(eegPowers["lowBeta"].ToString()));
					}
				  if(UpdateHighBetaEvent != null){
					 UpdateHighBetaEvent(float.Parse(eegPowers["highBeta"].ToString()));
					}
				  if(UpdateLowGammaEvent != null){
					 UpdateLowGammaEvent(float.Parse(eegPowers["lowGamma"].ToString()));			
					}
				  if(UpdateHighGammaEvent != null){
					 UpdateHighGammaEvent(float.Parse(eegPowers["highGamma"].ToString()));
					}
	            }
	          }
	          else if(primary.Contains("rawEeg") && UpdateRawdataEvent != null)
					  UpdateRawdataEvent(int.Parse(primary["rawEeg"].ToString()));
	          else if(primary.Contains("blinkStrength") && UpdateBlinkEvent != null)
					  UpdateBlinkEvent(int.Parse(primary["blinkStrength"].ToString()));
	        }
	      }
	      catch(IOException e)      { Debug.Log("NeuroSky Plugin IOException " + e); }
	      catch(System.Exception e) { Debug.Log("NeuroSky Plugin Exception " + e); }
	    }		
		
	}
	
	void OnApplicationQuit(){
		Disconnect();
	}

	
}
