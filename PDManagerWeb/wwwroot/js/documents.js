const products = document.getElementById("products"),
    formats = document.getElementById("formats"),
    lastModified = document.getElementById("lastModified"),
    lastModifier = document.getElementById("lastModifier"),
    uploadFile = document.getElementById("uploadFile"),
    reqAccBut = document.getElementById("reqAccBut"),
    uploadBut = document.getElementById("uploadBut"),
    downloadBut = document.getElementById("downloadBut");

function Startup() {

    LoadContents();
    products.addEventListener('change', LoadCurrent);
    formats.addEventListener('change', LoadCurrent);
    reqAccBut.addEventListener('click', ReqAccess);
    uploadBut.addEventListener('click', UplFile);
    downloadBut.addEventListener('click', DowFile);
    uploadFile.addEventListener('change', ToggleUplBut);
    LoadCurrent();
}

function ClearFile() {
    uploadFile.value = null;
    uploadBut.disabled = true;
}

function ToggleUplBut() {
    uploadBut.disabled = uploadFile.files.length === 0;
}

async function LoadContents() {
    const request = new Request("/api/ProgramDocuments/Contents");

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    products.innerHTML = "<option value='' disabled selected hidden>-Выберите продукт-</option>";

    json.products.forEach(function (product) {
        const option = document.createElement('option');
        option.value = product.id;
        option.innerHTML = product.name;
        products.appendChild(option);
    });

    formats.innerHTML = "<option value='' disabled selected hidden>-Выберите файл-</option>";

    json.formats.forEach(function (type) {
        const optGr = document.createElement('optgroup');
        optGr.label = type.name;
        type.formats.forEach(function (format) {
            const option = document.createElement('option');
            option.value = format.id;
            option.innerHTML = type.name + '.' + format.extension;
            optGr.appendChild(option);
        });
        formats.appendChild(optGr);
    });
}

async function LoadCurrent() {
    lastModified.innerHTML = "-";
    lastModifier.innerHTML = "-";
    ClearFile();
    uploadFile.disabled = true;
    reqAccBut.disabled = true;
    downloadBut.disabled = true;

    if (products.value === "" || formats.value === "") {
        return;
    }

    const request = new Request(`/api/ProgramDocuments/Status/${products.value}/${formats.value}`);

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    switch (json.result) {
        case -1:
            {
                LoadContents();
                break;
            }
        case 3:
            {
                lastModified.innerHTML = json.lastChangeDate;
                lastModifier.innerHTML = json.login;
                downloadBut.disabled = false;
            }
        case 2:
            {
                uploadFile.disabled = false;
                uploadFile.accept = '.' + json.extension;
                break;
            }
        case 1:
            {
                reqAccBut.disabled = false;
            }
        default: break;
    }
}

async function DowFile() {
    if (products.value === "" || formats.value === "") {
        return;
    }

    const request = new Request(`/api/ProgramDocuments/Files/${products.value}/${formats.value}`);

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }
    const json = await response.json();

    if (json.result === 0) {
        LoadContents();
    }
    else {
        const file = new File([base64ToArrayBuffer(json.file.fileContents)],
            { type: json.file.contentType });
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(file);
        link.download = json.file.fileDownloadName;
        link.click();
        window.URL.revokeObjectURL(link.href);
    }
}

function base64ToArrayBuffer(base64) {
    const binaryString = window.atob(base64);
    const binaryLen = binaryString.length;
    const bytes = new Uint8Array(binaryLen);
    for (let i = 0; i < binaryLen; i++) {
        const ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}

async function ReqAccess() {
    if (products.value === "") {
        return;
    }

    const request = new Request('/api/ProductAccesses/RequestAccess');

    const data = new FormData();
    data.append('productId', products.value);

    const options = {
        method: "POST",
        body: data
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadContents();
    }
    else {
        LoadCurrent();
    }
}

async function UplFile() {
    if (products.value === "" || formats.value === "" || uploadFile.files.length === 0) {
        return;
    }

    const request = new Request(`/api/ProgramDocuments/Files/${products.value}/${formats.value}`);

    const data = new FormData();
    data.append('uplfile', uploadFile.files[0]);

    const options = {
        method: "PUT",
        body: data
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    ClearFile();

    if (json.result === 0) {
        LoadContents();
    }
    else {
        lastModified.innerHTML = json.lastChangeDate;
        lastModifier.innerHTML = json.login;
        downloadBut.disabled = false;
    }
}

document.addEventListener('DOMContentLoaded', Startup);