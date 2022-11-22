using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class PlayerOnRewind : MonoBehaviour
{

    private List<Vector3> _rewindList = new List<Vector3>();
    private PlayerController _playerController;

    private Boolean _isIntermediateStaticClone=false;
    private Transform _transform;
    // Start is called before the first frame update

    [SerializeField] private PlayerOnRewind _intermediateStaticClone;
    private PlayerOnRewind _intermediateStaticCloneObject;

    void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
           if (! _isIntermediateStaticClone)
           {
            Rewind();

            if (Input.GetKeyDown(KeyCode.B)){
                _intermediateStaticCloneObject=Instantiate(_intermediateStaticClone,_transform.position,_transform.rotation);
                _intermediateStaticCloneObject.setIntermediateCloneBoolean(true);

                }
           }
    }

    private void Rewind()
    {


        int l = _rewindList.Count;
        if (l > 0)
        {
            // Debug.Log(l);
            Vector3 to_move = _rewindList[l - 1];
            _rewindList.RemoveAt(l - 1);
            //_rigidbody2D.MovePosition(to_move);
            _transform.position = to_move;

        }
        else
        {
            //_rigidbody2D.MovePosition(_transform.position);
            Destroy(gameObject);
            _playerController.StopRewinding();
        }

    }

    public void setRewindList(List<Vector3> rewindList)
    {
        _rewindList = rewindList;
    }
    public void setPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void setIntermediateCloneBoolean(Boolean boolean){
        _isIntermediateStaticClone=boolean;
    }



public PlayerOnRewind getStaticCloneOfThisClone(){
    return _intermediateStaticCloneObject;
}

}


