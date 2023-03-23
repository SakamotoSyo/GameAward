using UnityEngine;

public class MeshManager : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Mesh _myMesh;
    private Vector3[] _myVertices = default;

    [SerializeField, Tooltip("���_��")]
    private int _nVertices = 6;

    [SerializeField, Tooltip("���S��x���W")]
    private float _x0 = 0f;

    [SerializeField, Tooltip("���S��y���W")]
    private float _y0 = 0f;

    [SerializeField, Tooltip("�傫��"), Range(1, 10)]
    private float _radius = 2f;

    private int _indexNum = default;

    private float _dis = 1000f;

    Vector3 _closeMesh;

    void Start()
    {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _myMesh = new Mesh();

        _myVertices = new Vector3[_nVertices];

        Vector3[] myNormals = new Vector3[_nVertices];

        // ��ӓ�����̒��S�p�� 1 / 2
        float halfStep = Mathf.PI / _nVertices;

        for (int i = 0; i < _nVertices; i++)
        {
            // ���S���� i �Ԗڂ̒��_�Ɍ������p�x
            float angle = (i + 1) * halfStep;

            float x = _radius * Mathf.Cos(angle);

            float y = _radius * Mathf.Sin(angle);
            // �����̒��_�̈ʒu�Ɩ@��
            _myVertices[i].Set(_x0 - x, _y0 - y, 0);
            myNormals[i] = Vector3.forward;
            i++;
            // �Ō�̒��_�𐶐�������I��
            if (i >= _nVertices) break;
            // �㑤�̒��_�̈ʒu�Ɩ@��
            _myVertices[i].Set(_x0 - x, _y0 + y, 0);
            myNormals[i] = Vector3.forward;
        }
        _myMesh.SetVertices(_myVertices);

        _myMesh.SetNormals(myNormals);

        int nPolygons = _nVertices - 2;
        int nTriangles = nPolygons * 3;

        int[] myTriangles = new int[nTriangles];

        for (int i = 0; i < nPolygons; i++)
        {
            // �P�ڂ̎O�p�`�̍ŏ��̒��_�̒��_�ԍ��̊i�[��
            int firstI = i * 3;
            // �P�ڂ̎O�p�`�̒��_�ԍ�
            myTriangles[firstI + 0] = i;
            myTriangles[firstI + 1] = i + 1;
            myTriangles[firstI + 2] = i + 2;
            i++;
            // �Ō�̒��_�ԍ����i�[������I��
            if (i >= nPolygons) break;
            // �Q�ڂ̎O�p�`�̒��_�ԍ�
            myTriangles[firstI + 3] = i + 2;
            myTriangles[firstI + 4] = i + 1;
            myTriangles[firstI + 5] = i;
        }

        _myMesh.SetTriangles(myTriangles, 0);

        _meshFilter.mesh = _myMesh;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            for (int i = 0; i < _myVertices.Length; i++)
            {
                float dis = Vector3.Distance(worldPos, _myVertices[i]);
                if (dis < _dis)
                {
                    _dis = dis;
                    _closeMesh = _myVertices[i];
                    _indexNum = i;
                    Debug.Log(_myVertices[i]);
                }
            }

            float disX = worldPos.x - _closeMesh.x;
            float disY = worldPos.y - _closeMesh.y;

            if (disX < _radius && disY < _radius)
            {
                _closeMesh -= new Vector3(disX, disY, 0);
                _myVertices[_indexNum] = _closeMesh;

                Debug.Log($"��ԋ߂����_{_indexNum}��{disX}��{disY}�𑫂���{_myVertices[_indexNum].y}");

                _myMesh.SetVertices(_myVertices);
            }
            else
            {
                Debug.Log($"�@�����ꏊ����ԋ߂����_{_indexNum}���痣�ꂷ���Ă܂�");
            }
            _dis = 1000f;
        }
    }
}