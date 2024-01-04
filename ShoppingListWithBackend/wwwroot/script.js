const model = {
    inputs: {
        text: null,
    },
};

async function UpdateView() {
    document.getElementById('app').innerHTML = /*HTML*/ `
        <div>
            <h1>Shopping List</h1>
            <input 
                type="text" 
                placeholder="Enter item name here.."
                id="shoppingInput"
                value="${model.inputs.text || ''}"
            />
            <button id="addBtn" onclick="CreateListItem()">Add to list</button>
            <ol id="shoppingList">
            </ol>
        </div>
    `;

    document.getElementById('shoppingInput').addEventListener('input', function () {
        model.inputs.text = this.value;
    });

    const response = await axios.get('/shoppingListItems');
    model.textObjects = response.data;
    renderShoppingList();
}

async function CreateListItem() {
    const newItemText = model.inputs.text;

    if (newItemText) {
        const response = await axios.post('/shoppingListItems', { Text: newItemText });
        model.textObjects.push(response.data);
        renderShoppingList();
        document.getElementById('shoppingInput').value = '';
        model.inputs.text = '';
    }
}

async function removeListItem(id) {
    await axios.delete(`/shoppingListItems/${id}`);
    model.textObjects = model.textObjects.filter(item => item.id !== id);
    renderShoppingList();
}

function renderShoppingList() {
    const shoppingList = document.getElementById('shoppingList');
    shoppingList.innerHTML = '';
    model.textObjects.forEach(item => {
        const newListItem = document.createElement('li');
        newListItem.innerHTML = `<button class="deleteBtn" onclick="removeListItem('${item.id}')">Delete</button> <label>${item.text}</label>`;
        shoppingList.appendChild(newListItem);
    });
}

UpdateView();
