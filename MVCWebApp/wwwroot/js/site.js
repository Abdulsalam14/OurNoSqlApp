function showPdf(fileUrl) {
    var pdfViewer = document.getElementById('pdfViewer');
    pdfViewer.src = fileUrl;
    pdfViewer.style.display = 'block';
}