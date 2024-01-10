using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBook : MonoBehaviour, ISaveable
{
    [SerializeField] private BookPage[] pages;
    [SerializeField] private Animator animator;
    private int activePageNoLeft;

    private void Start() {
        pages = FindObjectsOfType(typeof(BookPage)) as BookPage[];
        Array.Sort(pages, new PageSorter());
        //activePageNoLeft = 0;
        safeActivate(0, true);
        //safeActivate(1, true);
        for (int i = 1; i < pages.Length; i++) {
            safeActivate(i, false);
        }
    }

    private void safeActivate(int i, bool val) {
        if (i >= 0 && i < pages.Length && pages[i])
            pages[i].gameObject.SetActive(val);
    }

    public class PageSorter : IComparer<BookPage> {

        public int Compare(BookPage x, BookPage y) {
            return x.pageNo < y.pageNo ? -1 : 1;
        }
    }

    public bool FlipTo(int pageNo) {
        if (pageNo < 0 || pageNo >= pages.Length + 1) {
            return false;
        }
        safeActivate(activePageNoLeft - 1, false);
        safeActivate(activePageNoLeft, false);
        activePageNoLeft = pageNo;
        safeActivate(activePageNoLeft - 1, true);
        safeActivate(activePageNoLeft, true);
        //animator.Play("PageFlip");
        return true;
    }

    public void updateEntryComplete(ConcoctionSO concoction, bool complete) {
        foreach (BookPage page in pages) {
            if (page is ConcoctionPage && ((ConcoctionPage) page).concoction == concoction) {
                if (complete) page.fillContents();
                else page.fillIncompleteContents();
            }
        }
    }

    public bool PrevPage() => FlipTo(activePageNoLeft - 2);

    public bool NextPage() => FlipTo(activePageNoLeft + 2);

    public void Save(SaveData data)
    {
        data.PageNoLeft = activePageNoLeft;
    }

    public void Load(SaveData data)
    {
        //activePageNoLeft = data.PageNoLeft;
        FlipTo(data.PageNoLeft);
    }
}
