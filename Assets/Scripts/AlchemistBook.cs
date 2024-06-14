using System;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBook : MonoBehaviour, ISaveable
{
    [SerializeField] private BookPage[] pages;
    private PageAnimationManager _pageAnimationManager;
    private int activePageNoLeft;

    private void Start()
    {
        _pageAnimationManager = GetComponent<PageAnimationManager>();
        pages = FindObjectsOfType(typeof(BookPage)) as BookPage[];
        foreach (BookPage page in pages)
            page.Initialise();

        Array.Sort(pages, new PageSorter());

        SafeActivate(0, true);
        for (int i = 1; i < pages.Length; i++) {
            SafeActivate(i, false);
        }
    }

    private void SafeActivate(int i, bool val) {
        if (i >= 0 && i < pages.Length && pages[i])
            pages[i].gameObject.SetActive(val);
    }

    public class PageSorter : IComparer<BookPage> {

        public int Compare(BookPage x, BookPage y) {
            return x.pageNo < y.pageNo ? -1 : 1;
        }
    }

    public bool GoToPage(int pageNo, bool prev)
    {
        if (pageNo % 2 == 1)
            return FlipTo(pageNo + 1, prev);
        else
            return FlipTo(pageNo, prev);
    }

    private bool FlipTo(int pageNo, bool prev) {
        if (pageNo < 0 || pageNo >= pages.Length + 1) {
            return false;
        }
        var currentPage = activePageNoLeft;
        if (prev)
        {
            _pageAnimationManager.AddLeftFlip();
            CallAfterDelay.Create(.2f, () =>
            {
                SafeActivate(currentPage, false);
                SafeActivate(pageNo, true);
            });
            SafeActivate(activePageNoLeft - 1, false);
            activePageNoLeft = pageNo;
            SafeActivate(activePageNoLeft - 1, true);
        }
        else
        {
            _pageAnimationManager.AddRightFlip();
            CallAfterDelay.Create(.2f, () => {
                SafeActivate(currentPage - 1, false);
                SafeActivate(pageNo - 1, true);
            });
            SafeActivate(activePageNoLeft, false);
            activePageNoLeft = pageNo;
            SafeActivate(activePageNoLeft, true);
        }
        return true;
    }

    public void UpdateEntry(ConcoctionSO concoction, bool complete) {
        foreach (BookPage page in pages) {
            if (page is ConcoctionPage page1 && page1.concoction == concoction)
            {
                if (complete) page.FillContents();
                else page.FillIncompleteContents();
            }
        }
    }

    public bool PreviousPage() => FlipTo(activePageNoLeft - 2, true);

    public bool NextPage() => FlipTo(activePageNoLeft + 2, false);

    public void PreviousPageAction() => PreviousPage();
    public void NextPageAction() => NextPage();

    public void HoverNextPage()
    {

    }

    public void HoverPreviousPage()
    {

    }

    public void Save(SaveData data)
    {
        data.PageNoLeft = activePageNoLeft;
    }

    public void Load(SaveData data)
    {
        FlipTo(data.PageNoLeft, false);
    }
}
