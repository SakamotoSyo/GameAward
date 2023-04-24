using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSample : MonoBehaviour
{
    [SerializeField] MeshManager _meshManager;

    [SerializeField, Tooltip("�匕�̃T���v��")]
    private List<Vector3> _taikenSample;

    [SerializeField, Tooltip("�o���̃T���v��")]
    private List<Vector3> _soukenSample;

    [SerializeField, Tooltip("�n���}�[�̃T���v��")]
    private List<Vector3> _hammerSample;

    [SerializeField, Tooltip("���̃T���v��")]
    private List<Vector3> _yariSample;

    public void SampleTaiken()
    {
        for (int i = 0; i < _taikenSample.Count; i++)
        {
            _meshManager.MyVertices[i] = _taikenSample[i];
            _meshManager.MyMesh.SetVertices(_taikenSample);
        }
    }
}
