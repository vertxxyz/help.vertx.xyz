let dropdown, label, options, bitcontainers;
// let codeNothing, codeEverything, codeMask; // layermask__code--none, layermask__code--everything, layermask__code--mask
let state = {};
state.isOpen = false;
state.mask = 0;
let optionNames = ["Nothing", "Everything", "Default", "TransparentFX", "Ignore Raycast", null, "Water", "UI"];

var reload = () => {
    dropdown = document.querySelector("#layermask-dropdown");
    label = dropdown.querySelector(":scope > .control-dropdown__label");
    label.onclick = toggle;

    let option = dropdown.querySelector(".control-dropdown__option");
    let parent = option.parentNode;

    options = new Array(32 + 2);
    options[0] = option;
    for (let i = 0; i < 34; i++) {
        options[i] = option.cloneNode(true);
        options[i].childNodes[2].nodeValue = i < optionNames.length && optionNames[i] != null ? optionNames[i] : `Layer ${i - 2}`;
    }

    parent.replaceChildren(...options);

    for (let i in options) {
        options[i].onclick = e => {
            e.preventDefault();
            check(i, options[i]);
        };
    }

    bitcontainers = new Array(32);
    let bit = document.querySelector("#layermask-bitmask").querySelector(".bitmask__bit-container");
    parent = bit.parentNode;
    for (let i = 0; i < 32; i++) {
        bitcontainers[i] = bit.cloneNode(true);
        bitcontainers[i].onclick = e => {
            e.preventDefault();
            check(i + 2);
        }
    }
    parent.replaceChildren(...bitcontainers);

    checkFromMask();

    window.addEventListener('click', ({target}) => {
        if (dropdown == null) return;
        const query = target.closest('#layermask-diagram');
        if (query == null)
            toggleTo(false);
    });
};

fireCallbackIfPageIsCurrent(reload);
reload();

function toggle(e) {
    e.preventDefault();
    toggleTo(!state.isOpen);
}

function toggleTo(toState) {
    if (state.isOpen === toState)
        return;
    state.isOpen = toState;
    if (toState) {
        dropdown.classList.add("on");
        for (let option of options)
            option.tabIndex = 0;
        options[0].focus();
    } else {
        dropdown.classList.remove("on");
        for (let option of options)
            option.tabIndex = -1;
        dropdown.querySelector("label").focus();
    }
}

function check(index) {
    switch (+index) { // lol javascript
        case 0: // Nothing
            state.mask = 0;
            checkFromMask();
            break;
        case 1: // Everything
            state.mask = ~0;
            checkFromMask();
            break;
        default: // Toggle one
            let checked = !options[index].firstElementChild.checked;
            options[index].firstElementChild.checked = checked;
            bitcontainers[index - 2].innerText = checked ? "1" : "0";
            bitcontainers[index - 2].classList.toggle("set", checked);
            state.mask ^= 1 << (index - 2);
            checkFromMask();
            /*if (state.mask === 0 || state.mask === ~0)
                checkFromMask();
            else {
                options[0].firstElementChild.checked = false;
                options[1].firstElementChild.checked = false;
                label.innerText = "Mixed...";
                // TODO handle single checked values without setting the whole mask
            }*/
            break;
    }
}

function checkFromMask() {
    if (state.mask === 0) {
        options[0].firstElementChild.checked = true;
        options[1].firstElementChild.checked = false;
        for (let i = 0; i < 32; i++) {
            options[i + 2].firstElementChild.checked = false;
            bitcontainers[i].innerText = "0";
            bitcontainers[i].classList.toggle("set", false);
        }
        label.innerText = "Nothing";
    } else if (state.mask === ~0) {
        options[1].firstElementChild.checked = true;
        options[0].firstElementChild.checked = false;
        for (let i = 0; i < 32; i++) {
            options[i + 2].firstElementChild.checked = true;
            bitcontainers[i].innerText = "1";
            bitcontainers[i].classList.toggle("set", true);
        }
        label.innerText = "Everything";
    } else {
        options[0].firstElementChild.checked = false;
        options[1].firstElementChild.checked = false;
        let checkedCount = 0;
        let lastCheckedIndex = 0;
        for (let i = 0; i < 32; i++) {
            let checked = (state.mask & (1 << i)) !== 0;
            options[i + 2].firstElementChild.checked = checked;
            bitcontainers[i].innerText = checked ? "1" : "0";
            bitcontainers[i].classList.toggle("set", checked);
            if (checked) {
                checkedCount++;
                lastCheckedIndex = i;
            }
        }
        if (checkedCount === 1)
            label.innerText = options[lastCheckedIndex + 2].childNodes[2].nodeValue;
        else
            label.innerText = "Mixed...";
    }
}