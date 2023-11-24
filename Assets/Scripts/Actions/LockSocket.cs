using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.Assertions;
namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
    public class LockSocket : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
    {
        #region Properties
        [SerializeField]
        private XRSocketInteractor m_interactor;
        [SerializeField]
        private bool m_locked = false;

        public bool locked { get => m_locked; set => m_locked = value; }

        public bool canProcess => true;

        #endregion
        private void Awake()
        {
            m_interactor = m_interactor ?? GetComponent<XRSocketInteractor>();
            Assert.IsNotNull(m_interactor);
        }
        public void LockCurrentSocket()
        {
            var interactable = m_interactor.GetOldestInteractableSelected().transform.GetComponent<XRBaseInteractable>();
            if (interactable == null)
                return;

            // Add filter to interactable
            interactable.hoverFilters.Add(this);
            interactable.selectFilters.Add(this);
        }

        public void UnlockCurrentSocket()
        {
            var interactable = m_interactor.GetOldestInteractableSelected().transform.GetComponent<XRBaseInteractable>();
            if (interactable == null)
                return;

            // Remove filter from interactable
            interactable.hoverFilters.Remove(this);
            interactable.selectFilters.Remove(this);
        }

        public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
        {
            return Process();
        }

        public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            return Process();
        }
        private bool Process()
        {
            if (m_interactor == null)
                return false;

            return !m_locked;
        }
    }

}