export function saveAsFile(fileName, byteBase64) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
export function descargarArchivo(url, nombreArchivo) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = nombreArchivo ?? '';
    document.body.appendChild(anchorElement);
    anchorElement.click();
    document.body.removeChild(anchorElement);
}