function openAddUserModal() {
    document.getElementById('userForm').reset();
    document.getElementById('userModalLabel').innerText = 'Add User';
    document.getElementById('modalSubmitButton').innerText = 'Add User';
    document.getElementById('userId').value = '';

    const userModal = new mdb.Modal(document.getElementById('userModal'));
    userModal.show();
}

function openEditUserModal(userId, firstName, lastName, email, phoneNumber, roleId) {
    document.getElementById('userId').value = userId;
    document.getElementById('firstName').value = firstName;
    document.getElementById('lastName').value = lastName;
    document.getElementById('email').value = email;
    document.getElementById('phoneNumber').value = phoneNumber;
    document.getElementById('role').value = roleId;

    document.getElementById('userModalLabel').innerText = 'Edit User';
    document.getElementById('modalSubmitButton').innerText = 'Save Changes';

    const userModal = new mdb.Modal(document.getElementById('userModal'));
    userModal.show();
}

function deleteUser(userId) {
    if (confirm('Are you sure you want to delete this user?')) {
        $.ajax({
            url: '/User/DeleteUser', // Adjust the URL to match your controller's route
            type: 'POST',
            data: { id: userId },
            success: function (result) {
                if (result.success) {
                    alert('User deleted successfully.');
                    // Optionally, remove the user from the UI or reload the page
                    location.reload();
                } else {
                    alert('Error: ' + result.error);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + error);
            }
        });
    }
}
