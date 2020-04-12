[System.Serializable]
public class PID
{
	public float pFactor = 4;
	public float iFactor = 0.007f;
	public float dFactor = 0f;

	float integral;
	float lastError;


	public PID(float pFactor, float iFactor, float dFactor)
	{
		this.pFactor = pFactor;
		this.iFactor = iFactor;
		this.dFactor = dFactor;
	}

	public void SetValues(float integral,float lastError)
	{
		this.integral = integral;
		this.lastError = lastError;
	}

	public float Update(float setpoint, float actual, float timeFrame)
	{
		float present = setpoint - actual;
		integral += present * timeFrame;
		float deriv = (present - lastError) / timeFrame;
		lastError = present;
		return present * pFactor + integral * iFactor + deriv * dFactor;
	}
}

