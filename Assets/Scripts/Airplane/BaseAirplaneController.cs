using UnityEngine;

[RequireComponent(typeof(Airplane))]
public class BaseAirplaneController : MonoBehaviour
{
    #region Fields & Properties

    protected Airplane airplane = null;

    #endregion Fields & Properties

    // Start is called before the first frame update
    protected virtual void Start()
    {
        this.airplane = this.GetComponent<Airplane>();
    }
}
