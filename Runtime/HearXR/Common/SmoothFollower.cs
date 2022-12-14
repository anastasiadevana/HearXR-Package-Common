//------------------------------------------------------------------------------
// Smooth Follower
//------------------------------------------------------------------------------
//
// Copyright (c) 2022 Anastasia Devana
//
// May be used for reference purposes only. Contact author for any intended
// duplication or intended use beyond exploratory read, compilation, testing.
// No license or rights transfer implied from the open publication. This
// software is made available strictly on an "as is" basis without warranty of
// any kind, express or implied.
//------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace HearXR.Common
{
    public class SmoothFollower : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private Transform _objectToFollow;
        [SerializeField] private bool _followMainCamera;
        
        [Header("Position")] 
        public bool followPosition = true;
        public bool smoothFollowPosition;
        [Tooltip("Lower numbers are slower")] 
        public float smoothPositionFollowSpeed = 0.2f;
        
        [Header("Rotation")] public bool followRotation = true;
        public bool smoothFollowRotation;
        [Tooltip("Lower numbers are slower")]
        public float smoothRotationFollowSpeed = 0.2f;
        #endregion
        
        #region Properties
        public Transform ObjectToFollow
        {
            get => _objectToFollow;
            set
            {
                _objectToFollow = value;
                _hasObjectToFollow = (_objectToFollow != null);
            }
        }

        public bool HasObjectToFollow => _hasObjectToFollow;

        public bool Follow
        {
            get => _follow;
            set => _follow = value;
        }
        #endregion

        #region Private Fields
        private bool _hasObjectToFollow;
        private bool _follow = true;
        #endregion

        #region Init
        private void Awake()
        {
            _hasObjectToFollow = (_objectToFollow != null);
        }

        private void Start()
        {
            if (_followMainCamera)
            {
                _objectToFollow = Camera.main.transform;
                _hasObjectToFollow = true;
            } 
        }
        #endregion
        
        #region Loop
        private void Update()
        {
            if (!_follow || !_hasObjectToFollow) return;

            // TODO: Test what kind of exception will crop up, so we can just handle that one.
            // if (_hasObjectToFollow)
            // {
            //     Destroy(_objectToFollow.gameObject);
            // }
            
            // Since the target object can get destroyed when we least expect it, basically just always expect it.
            try
            {
                if (followPosition) FollowObjectPosition();
                if (followRotation) FollowObjectRotation();
            }
            catch (Exception e)
            {
                _objectToFollow = null;
                _hasObjectToFollow = false;
                Debug.Log(e);
                throw;
            }
        }
        #endregion
        
        #region Private Methods
        private void FollowObjectPosition()
        {
            Vector3 targetPosition = _objectToFollow.position;
            transform.position = (smoothFollowPosition)
                ? Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothPositionFollowSpeed)
                : targetPosition;
        }

        private void FollowObjectRotation()
        {
            Quaternion targetRotation = _objectToFollow.rotation;
            transform.rotation = (smoothFollowRotation)
                ? Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotationFollowSpeed)
                : targetRotation;
        }
        #endregion
    }   
}
