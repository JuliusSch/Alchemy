using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentsMenuButton : PageButton
{
    [SerializeField] private AlchemistBook _book;
    [SerializeField] private int _targetPage;
    [SerializeField] private TMP_Text _pageNoField, _pageTitleField;
    [SerializeField] private string _title;

    private void Awake()
    {
        _pageNoField.text = _targetPage.ToString() + ".";
        _pageTitleField.text = _title;
    }

    public void GoToPage()
    {
        _book.GoToPage(_targetPage, false);
        //pagenof
    }
}
