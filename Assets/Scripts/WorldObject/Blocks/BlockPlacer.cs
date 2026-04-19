using UnityEngine;

[ExecuteAlways]
public class BlockPlacer : MonoBehaviour
{
#if UNITY_EDITOR
    private float _zStep = 0.25f;
    private float _xStep = 0.15f;
    private float _yPosition = 0f;
    private float _xBorder = 0.75f;

    private void Update()
    {
        if (Application.isPlaying == true)
            return;

        if (transform.hasChanged && !Input.GetMouseButton(0))
        {
            var fixedPosition = transform.position;

            var zPozition = fixedPosition.z;

            var zResidue = zPozition % _zStep;

            if (zResidue >= _zStep / 2)
                zPozition = zPozition + (_zStep - zResidue);
            else
                zPozition = zPozition - zResidue;


            var xPozition = fixedPosition.x;

            var xResidue = xPozition % _xStep;

            if (Mathf.Abs(xResidue) >= _xStep / 2)
                xPozition = xPozition + ((_xStep * Mathf.Sign(xResidue)) - xResidue);
            else
                xPozition = xPozition - xResidue;


            fixedPosition.z = zPozition;
            fixedPosition.x = xPozition;
            fixedPosition.y = _yPosition;

            if (fixedPosition.x > _xBorder)
                fixedPosition.x = _xBorder;
            else if (fixedPosition.x < -_xBorder)
                fixedPosition.x = -_xBorder;

            transform.position = fixedPosition;
        }
    }
#endif
}
