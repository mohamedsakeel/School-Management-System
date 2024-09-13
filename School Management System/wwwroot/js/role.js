function editRole(roleId, roleName) {

    // Set the modal title and values based on whether we are editing or adding
    document.getElementById('roleModalLabel').innerText = roleId ? 'Edit Role' : 'Add Role';
    document.getElementById('roleId').value = roleId;
    document.getElementById('roleName').value = roleName;

    const roleModal = new mdb.Modal(document.getElementById('roleModal'));
    roleModal.show();
}

function openAddRoleModal() {
    document.getElementById('roleform').reset();
    document.getElementById('roleModalLabel').innerText = 'Add Role';
    document.getElementById('modalsubmitbtn').innerText = 'Add Role';
    document.getElementById('roleId').value = '';

    const roleModal = new mdb.Modal(document.getElementById('roleModal'));
    roleModal.show();
}

