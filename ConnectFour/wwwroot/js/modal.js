var modal = document.getElementById("myModal");

var btn = document.getElementById("myBtn");

var span = document.getElementsByClassName("close")[0];



function copyToClipboard() {
    var copyText = document.getElementById("roomId");
    copyText.select();
    copyText.setSelectionRange(0, 99999);
    document.execCommand("copy");
    clearSelection();

    var tooltip = document.getElementById("myTooltip");
    tooltip.innerHTML = "Copied: " + copyText.value;
}

function outFunc() {
    var tooltip = document.getElementById("myTooltip");
    tooltip.innerHTML = "Copy to clipboard";
}

function goBack() {
    window.location.href = "/Index"; 
}

function clearSelection() {
    if (window.getSelection) { window.getSelection().removeAllRanges(); }
    else if (document.selection) { document.selection.empty(); }
}