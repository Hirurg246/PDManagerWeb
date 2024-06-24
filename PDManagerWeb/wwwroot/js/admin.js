const prAccs = document.getElementById("prAccs"),
    gAccB = document.getElementById("gAccB"),
    dAccB = document.getElementById("dAccB"),
    prN = document.getElementById("prN"),
    cPrB = document.getElementById("cPrB"),
    prs = document.getElementById("prs"),
    prCrDt = document.getElementById("prCrDt"),
    dPrB = document.getElementById("dPrB"),
    dtN = document.getElementById("dtN"),
    cDtB = document.getElementById("cDtB"),
    dts = document.getElementById("dts"),
    dDtB = document.getElementById("dDtB"),
    ffN = document.getElementById("ffN"),
    cFfB = document.getElementById("cFfB"),
    ffs = document.getElementById("ffs"),
    dFfB = document.getElementById("dFfB"),
    dfDts = document.getElementById("dfDts"),
    dfFfs = document.getElementById("dfFfs"),
    cDfB = document.getElementById("cDfB"),
    dDfB = document.getElementById("dDfB");

function Startup() {
    LoadPrs();
    LoadDts();
    LoadFfs();
    LoadPrAccs();
    prs.addEventListener('change', () => {
        if (prs.value === "") {
            prCrDt.innerHTML = "-";
            dPrB.disabled = true;
        }
        else {
            prCrDt.innerHTML = prs.options[prs.selectedIndex].dataset.st;
            dPrB.disabled = false;
        }
    });
    cPrB.addEventListener('click', CreatePr);
    dPrB.addEventListener('click', DeletePr);
    dts.addEventListener('change', () => { dDtB.disabled = dts.value === ""; });
    cDtB.addEventListener('click', CreateDt);
    dDtB.addEventListener('click', DeleteDt);
    ffs.addEventListener('change', () => { dFfB.disabled = ffs.value === ""; });
    cFfB.addEventListener('click', CreateFf);
    dFfB.addEventListener('click', DeleteFf);
    prAccs.addEventListener('change', () => {
        gAccB.disabled = prAccs.value === "";
        dAccB.disabled = gAccB.disabled;
    });
    gAccB.addEventListener('click', () => SetPrAcc(true));
    dAccB.addEventListener('click', () => SetPrAcc(false));
    dfDts.addEventListener('change', LoadDfState);
    dfFfs.addEventListener('change', LoadDfState);
    cDfB.addEventListener('click', () => SetDfState(true));
    dDfB.addEventListener('click', () => SetDfState(false));
}

async function LoadPrs() {
    prCrDt.innerHTML = "-";
    dPrB.disabled = true;

    const request = new Request("/api/Products/MainInfo");

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    prs.innerHTML = "<option value='' disabled selected hidden>-Выберите продукт-</option>";

    json.forEach(function (pr) {
        const option = document.createElement('option');
        option.value = pr.id;
        option.dataset.st = pr.startDate;
        option.innerHTML = pr.name;
        prs.appendChild(option);
    });
}

async function CreatePr() {
    if (prN.value === "") {
        return;
    }

    const request = new Request("/api/Products");

    const data = new FormData();
    data.append('productName', prN.value);

    const options = {
        method: "POST",
        body: data
    };

    prN.value = "";

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadPrs();
    }
    else {
        const option = document.createElement('option');
        option.value = json.id;
        option.dataset.st = json.startDate;
        option.innerHTML = json.name;
        prs.appendChild(option);
    }
}

async function DeletePr() {
    if (prs.value === "") {
        return;
    }

    const request = new Request(`/api/Products/${prs.value}`);

    const curInd = prs.selectedIndex;

    const options = {
        method: "DELETE"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadPrs();
    }
    else {
        prs.remove(curInd);
        prs.selectedIndex = 0;
        prs.dispatchEvent(new Event('change'));
    }
}

async function LoadDts() {
    dDtB.disabled = true;
    cDfB.disabled = true;
    dDfB.disabled = true;

    const request = new Request("/api/DocumentTypes");

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    dts.innerHTML = "<option value='' disabled selected hidden>-Выберите вид документации-</option>";
    dfDts.innerHTML = dts.innerHTML;

    json.forEach(function (dt) {
        const option = document.createElement('option');
        option.value = dt.id;
        option.innerHTML = dt.name;
        dts.appendChild(option);
        dfDts.appendChild(option.cloneNode(true));
    });
}

async function CreateDt() {
    if (dtN.value === "") {
        return;
    }

    const request = new Request("/api/DocumentTypes");

    const data = new FormData();
    data.append('docTypeName', dtN.value);

    const options = {
        method: "POST",
        body: data
    };

    dtN.value = "";

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadDts();
    }
    else {
        const option = document.createElement('option');
        option.value = json.id;
        option.innerHTML = json.name;
        dts.appendChild(option);
        dfDts.appendChild(option.cloneNode(true));
    }
}

async function DeleteDt() {
    if (dts.value === "") {
        return;
    }

    const request = new Request(`/api/DocumentTypes/${dts.value}`);

    const curInd = dts.selectedIndex;

    const options = {
        method: "DELETE"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadDts();
    }
    else {
        dts.remove(curInd);
        dts.selectedIndex = 0;
        dts.dispatchEvent(new Event('change'));
        dfDts.remove(curInd);
        dfDts.selectedIndex = 0;
        dfDts.dispatchEvent(new Event('change'));
    }
}

async function LoadFfs() {
    dFfB.disabled = true;
    cDfB.disabled = true;
    dDfB.disabled = true;

    const request = new Request("/api/DocumentFormats");

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    ffs.innerHTML = "<option value='' disabled selected hidden>-Выберите формат файла-</option>";
    dfFfs.innerHTML = ffs.innerHTML;

    json.forEach(function (ff) {
        const option = document.createElement('option');
        option.value = ff.id;
        option.innerHTML = ff.extension;
        ffs.appendChild(option);
        dfFfs.appendChild(option.cloneNode(true));
    });
}

async function CreateFf() {
    if (ffN.value === "") {
        return;
    }

    const request = new Request("/api/DocumentFormats");

    const data = new FormData();
    data.append('fileFormatName', ffN.value);

    const options = {
        method: "POST",
        body: data
    };

    ffN.value = "";

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadFfs();
    }
    else {
        const option = document.createElement('option');
        option.value = json.id;
        option.innerHTML = json.extension;
        ffs.appendChild(option);
        dfFfs.appendChild(option.cloneNode(true));
    }
}

async function DeleteFf() {
    if (ffs.value === "") {
        return;
    }

    const request = new Request(`/api/DocumentFormats/${dts.value}`);

    const curInd = ffs.selectedIndex;

    const options = {
        method: "DELETE"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadFfs();
    }
    else {
        ffs.remove(curInd);
        ffs.selectedIndex = 0;
        ffs.dispatchEvent(new Event('change'));
        dfFfs.remove(curInd);
        dfFfs.selectedIndex = 0;
        dfFfs.dispatchEvent(new Event('change'));
    }
}

async function LoadPrAccs() {
    gAccB.disabled = true;
    dAccB.disabled = true;

    const request = new Request("/api/ProductAccesses/Active");

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    prAccs.innerHTML = "<option value='' disabled selected hidden>-Выберите запрос-</option>";

    json.forEach(function (prAcc) {
        const option = document.createElement('option');
        option.value = prAcc.userId;
        option.dataset.pr = prAcc.productId;
        option.innerHTML = prAcc.userName + " к " + prAcc.productName;
        prAccs.appendChild(option);
    });
}

async function SetPrAcc(answer) {
    if (prAccs.value === "") {
        return;
    }

    const request = new Request("/api/ProductAccesses/Reply");

    const curInd = prAccs.selectedIndex;

    const data = new FormData();
    data.append('userId', prAccs.value);
    data.append('productId', prAccs.options[curInd].dataset.pr);
    data.append('answer', answer);

    const options = {
        method: "PUT",
        body: data
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    if (json.result === 0) {
        LoadPrAccs();
    }
    else {
        prAccs.remove(curInd);
        prAccs.selectedIndex = 0;
        prAccs.dispatchEvent(new Event('change'));
    }
}

async function LoadDfState() {
    cDfB.disabled = true;
    dDfB.disabled = true;

    if (dfDts.value === "" || dfFfs.value === "") {
        return;
    }

    const request = new Request(`/api/AllowedFormats/Status/${dfDts.value}/${dfFfs.value}`);

    const options = {
        method: "GET"
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    switch (json.result) {
        case -2:
            {
                LoadDts();
                break;
            }
        case -1:
            {
                LoadFfs();
                break;
            }
        case 0:
            {
                cDfB.disabled = false;
                break;
            }
        default:
            {
                dDfB.disabled = false;
                break;
            }
    }
}

async function SetDfState(state) {
    if (dfDts.value === "" || dfFfs.value === "") {
        return;
    }

    const request = new Request("/api/AllowedFormats/Set");

    const data = new FormData();
    data.append('docTypeId', dfDts.value);
    data.append('fileFormatId', dfFfs.value);
    data.append('state', state);

    const options = {
        method: "PUT",
        body: data
    };

    const response = await fetch(request, options);

    if (!response.ok) {
        return;
    }

    const json = await response.json();

    LoadDfState();
}

document.addEventListener('DOMContentLoaded', Startup);