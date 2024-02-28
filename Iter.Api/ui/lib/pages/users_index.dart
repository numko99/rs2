import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/role_helper.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/Agency/insertAgencyModal.dart';
import 'package:ui/modals/insert_user_modal.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/user.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/services/user_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';
import 'package:ui/widgets/search_button.dart';

import '../modals/customConfirmationModal.dart';

class UsersIndexPage extends StatefulWidget {
  const UsersIndexPage({super.key});

  @override
  State<UsersIndexPage> createState() => _UsersIndexPageState();
}

class _UsersIndexPageState extends State<UsersIndexPage> {
  UserProvider? _userProvider;
  DropdownProvider? _dropdownProvider;

  List<User> users = [];
  int? userCount;
  bool displayLoader = true;

  int _currentPage = 1;
  int pageSize = 3;
  TextEditingController searchController = TextEditingController();

  int? selectedUserType;
  final List<Map<int?, String>> userTypeDropdown = [
    {null: "Nije odabrano"},
    {2: "Koordinator"},
    {3: "Vodič"},
    {4: "Klijent"},
  ];

  String? selectedAgency;
  List<DropdownModel>? agenciesDropdown;

  @override
  void initState() {
    super.initState();
    _userProvider = context.read<UserProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      name: "Korisnici",
      icon: Icons.person_2,
      body: Card(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(45.0, 10, 45.0, 10),
          child: Column(
            children: <Widget>[
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  Expanded(
                    child: Row(
                      crossAxisAlignment: CrossAxisAlignment.end,
                      children: <Widget>[
                        Expanded(
                          flex:
                              2,
                          child: TextFormField(
                            controller: searchController,
                            decoration: const InputDecoration(
                              labelText: 'Ime i prezime',
                            ),
                            onFieldSubmitted: (value) async {
                              await search();
                            },
                          ),
                        ),
                        const SizedBox(width: 20),
                        Expanded(
                          flex: 2,
                          child: DropdownButtonFormField<int?>(
                              decoration: const InputDecoration(
                                labelText: 'Izaberite tip korisnik',
                              ),
                              value: selectedUserType,
                              items: userTypeDropdown.expand((item) {
                                return item.entries.map((entry) {
                                  return DropdownMenuItem<int?>(
                                    value: entry.key,
                                    child: Text(entry.value),
                                  );
                                });
                              }).toList(),
                              onChanged: (int? value) {
                                setState(() {
                                  selectedUserType = value;
                                });
                              }),
                        ),
                        const SizedBox(width: 20),
                       Expanded(
                          flex: 2,
                          child: DropdownButtonFormField<dynamic>(
                            decoration: const InputDecoration(
                              labelText: 'Izaberite agenciju',
                            ),
                            value:
                                selectedAgency,
                            items: agenciesDropdown?.map((DropdownModel item) {
                              return DropdownMenuItem<dynamic>(
                                value: item.id,
                                child: Text(item.name ?? ""),
                              );
                            }).toList(),
                            onChanged: (value) {
                              setState(() {
                                selectedAgency = value;
                              });
                            },
                          ),
                        ),
                        const SizedBox(width: 20),
                        Expanded(
                          flex: 1,
                          child: SearchButton(
                            onSearch: () => search(),
                          ),
                        ),
                        const Spacer(),
                        const Spacer(),
                      ],
                    ),
                  )
                ],
              ),
              const SizedBox(height: 10),
              Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  Container(
                    height: 35,
                    margin: const EdgeInsets.fromLTRB(0, 15, 0, 0),
                    child: ElevatedButton(
                      onPressed: () {
                        showDialog(
                          context: context,
                          builder: (BuildContext context) {
                            return InsertUserModal(onCompleted: () => initialLoad(), userTypeDropdown: userTypeDropdown, agenciesDropdown: agenciesDropdown);
                          },
                        );
                      },
                      child: const Text(
                        "Dodaj korisnika",
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                  )
                ],
              ),
              const SizedBox(height: 50),
              Column(
                children: [
                  if (displayLoader)
                    const CircularProgressIndicator()
                  else
                    Row(children: <Widget>[
                      Expanded(
                        child: DataTable(
                          columns: const [
                            DataColumn(
                                label: Text('Ime',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Prezime',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Email',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Tip korisnika',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(
                                label: Text('Agencija',
                                    style: TextStyle(
                                        fontWeight: FontWeight.bold))),
                            DataColumn(label: Text('')),
                          ],
                          rows: users
                              .map(
                                (user) => DataRow(
                                  cells: [
                                    DataCell(Text(user.firstName!)),
                                    DataCell(Text(user.lastName!)),
                                    DataCell(Text(user.email ?? "")),
                                    DataCell(
                                      Container(
                                        padding: const EdgeInsets.all(8.0),
                                        color: user.role == 2
                                            ? Colors.blue
                                            : user.role == 3
                                                ? Colors.green
                                                : Colors.amber,
                                        child: Text(
                                          RoleHelper.GetRoleName(user.role),
                                          style: const TextStyle(
                                              color: Colors.white),
                                        ),
                                      ),
                                    ),
                                    DataCell(Text(user.agency != null
                                        ? user.agency!.name
                                        : "")),
                                    DataCell(
                                      SizedBox(
                                        width: double.infinity,
                                        child: Row(
                                          mainAxisAlignment:
                                              MainAxisAlignment.end,
                                          // Align to the end (right)
                                          children: <Widget>[
                                            IconButton(
                                                icon: const Icon(Icons.open_in_new_off),
                                                onPressed: () {
                                                  Navigator.pushNamed(
                                                      context, '/user/details',
                                                      arguments: {
                                                        'id': user.id
                                                      });
                                                },
                                                tooltip: 'Detalji'),
                                            IconButton(
                                                icon: const Icon(
                                                    Icons.edit_document),
                                                onPressed: () {
                                                  showDialog(
                                                    context: context,
                                                    builder:
                                                        (BuildContext context) {
                                                      return InsertUserModal(
                                                          onCompleted: () => search(),
                                                          agenciesDropdown: agenciesDropdown,
                                                          userTypeDropdown: userTypeDropdown,
                                                          user: user);
                                                    },
                                                  );
                                                },
                                                tooltip: 'Uredi'),
                                                IconButton(
                                                icon: const Icon(Icons.password_outlined),
                                                onPressed: () {
                                                  showDialog(
                                                    context: context,
                                                    builder:
                                                        (BuildContext context) {
                                                      return CustomConfirmationDialog(
                                                        title:
                                                            "Potvrda promjene lozinke?",
                                                        content:
                                                            "Da li ste sigurni da želite generisati novu lozinku za korisnika ${user.firstName} ${user.lastName}?\nNapomena: Nakon potvrde, email sa novom lozinkom će biti proslijeđen korisniku.",
                                                        onConfirm: () async {
                                                          newPassword(user.id);
                                                        },
                                                      );
                                                    },
                                                  );
                                                },
                                                tooltip: 'Promijena lozinke'),  
                                            IconButton(
                                                icon: const Icon(Icons.delete),
                                                onPressed: () {
                                                  showDialog(
                                                    context: context,
                                                    builder:
                                                        (BuildContext context) {
                                                      return CustomConfirmationDialog(
                                                        title:
                                                            "Potvrda brisanja?",
                                                        content:
                                                            "Da li ste sigurni da želite obrisati korisnika ${user.firstName} ${user.lastName}",
                                                        onConfirm: () async {
                                                          await delete(user.id);
                                                        },
                                                      );
                                                    },
                                                  );
                                                },
                                                tooltip: 'Obriši'),
                                          ],
                                        ),
                                      ),
                                    ),
                                  ],
                                ),
                              )
                              .toList(),
                        ),
                      ),
                    ]),
                  const SizedBox(height: 20),
                  Row(
                    children: [
                      Expanded(
                        child: Align(
                          alignment: Alignment.center,
                          child: Container(
                            constraints: const BoxConstraints(maxWidth: 300),
                            child: Visibility(
                              visible:
                                  userCount != null && userCount! > pageSize,
                              child: NumberPaginator(
                                //TODO FIX NUMBER PAGINATOR
                                initialPage: 0,
                                numberPages:
                                    (((userCount == 0 || userCount == null)
                                                ? 1
                                                : userCount!) /
                                            (pageSize ?? 1))
                                        .ceil(),
                                onPageChange: (int index) {
                                  loadPage(index, pageSize);
                                },
                              ),
                            ),
                          ),
                        ),
                      )
                    ],
                  )
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> initialLoad() async {
    try {
      await search();

      var agenciesDropdownTemp = await _dropdownProvider!
          .get({"dropdownType": DropdownTypes.agencies});

      agenciesDropdownTemp.result
          .insert(0, new DropdownModel(id: null, name: "Nije odabrano"));

      if (agenciesDropdownTemp.result.isNotEmpty) {
        setState(() {
          agenciesDropdown = agenciesDropdownTemp.result;
          displayLoader = false;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    }
  }

  Future<void> search() async {
    try {
      setState(() {
        displayLoader = true;
      });
      var searchResult = await _userProvider?.get({
        "currentPage": _currentPage,
        "pageSize": pageSize,
        "name": searchController.text,
        "roleId": selectedUserType,
        "agencyId": selectedAgency
      });

      if (searchResult != null) {
        setState(() {
          users = searchResult.result;
          userCount = searchResult.count;
          displayLoader = false;
        });
      } else {
        setState(() {
          users = [];
          userCount = 0;
        });
      }
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  Future<void> loadPage(int page, int? pageSize) async {
    setState(() {
      _currentPage = page + 1;
    });
    await search();
  }

  Future<void> delete(id) async {
    try {
      setState(() {
        displayLoader = true;
      });
      await _userProvider?.delete(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste obrisali korisnika",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }

  void newPassword(String id) async {
    try {
      setState(() {
        displayLoader = true;
      });
      await _userProvider?.newPassword(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste promijenili lozinku korisniku",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }
}
