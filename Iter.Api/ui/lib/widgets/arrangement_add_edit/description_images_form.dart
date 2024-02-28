import 'dart:convert';
import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:ui/helpers/image_helper.dart';
import 'package:ui/models/image_model.dart';

class DescriptionImageFormPage extends StatefulWidget {
  final List<ImageModel>? images;
  final ImageModel? mainImage;
  final bool displayMainImageValidationMsg;
  final bool displayImagesValidationMsg;
  final Function(List<ImageModel>) onUpdateImages;
  final Function(ImageModel?) onUpdateMainImage;

  const DescriptionImageFormPage({
    super.key,
    required this.images,
    required this.mainImage,
    required this.displayImagesValidationMsg,
    required this.displayMainImageValidationMsg,
    required this.onUpdateImages,
    required this.onUpdateMainImage,
  });

  @override
  State<DescriptionImageFormPage> createState() => _DescriptionImageFormPageState();
}

class _DescriptionImageFormPageState extends State<DescriptionImageFormPage> {
  @override
  void initState() {
    super.initState();
  }

    Future getImages(bool allowMultiple) async {
    var result = await FilePicker.platform.pickFiles(
      type: FileType.image,
      allowMultiple: allowMultiple,
    );

    if (result != null && result.files.isNotEmpty) {
      var imagesTemp = await Future.wait(result.files.map((file) async {
        return await ImageHelper.processImage(File(file.path!));
      }));

      setState(() {
        if (allowMultiple) {
          widget.images?.addAll(imagesTemp);
          widget.onUpdateImages(widget.images!);
        } else {
          widget.onUpdateMainImage(imagesTemp[0]);
        }
      });
    }
  }
  

  @override
  Widget build(BuildContext context) {
    return Column(children: [
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Padding(
              padding: const EdgeInsets.fromLTRB(0, 6, 0, 0),
              child: FormBuilderTextField(
                  name: 'description',
                  maxLines: 10,
                  minLines: 1,
                  decoration: const InputDecoration(labelText: 'Opis'),
                  validator: FormBuilderValidators.compose([
                    FormBuilderValidators.required(
                        errorText: "Polje je obavezno"),
                  ])),
            ),
          ),
        ],
      ),
      const SizedBox(height: 20),
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              children: [
                FormBuilderField(
                  name: 'mainImage',
                  builder: ((field) {
                    return InputDecorator(
                      decoration: InputDecoration(
                          label: Text('Odaberite pozadinu'),
                          errorText: field.errorText),
                      child: ListTile(
                        leading: Icon(Icons.photo),
                        title: Text("Glavna slika"),
                        trailing: Icon(Icons.file_upload),
                        onTap: () => getImages(false),
                      ),
                    );
                  }),
                ),
                if (widget.displayMainImageValidationMsg) ...[
                  Row(
                    children: [
                      Padding(
                        padding: EdgeInsets.fromLTRB(0, 5, 0, 5),
                        child: Text(
                          "Molimo odaberite sliku",
                          style: TextStyle(
                            fontSize: 12.0,
                            color: Colors.red[700],
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
                if (widget.mainImage != null) ...[
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Container(
                        padding: EdgeInsets.fromLTRB(0, 15, 0, 0),
                        width: 100,
                        height: 100,
                        child: Image.memory(base64Decode(widget.mainImage!.image)),
                      ),
                      IconButton(
                        icon: Icon(Icons.close),
                        onPressed: () {
                          setState(() {
                            widget.onUpdateMainImage(null);
                          });
                        },
                      ),
                    ],
                  ),
                ]
              ],
            ),
          ),
          const SizedBox(width: 60),
          Expanded(
              flex: 2,
              child: Column(children: [
                FormBuilderField(
                  name: 'images',
                  builder: ((field) {
                    return InputDecorator(
                      decoration: InputDecoration(
                          label: Text('Odaberite slike putovanja'),
                          errorText: field.errorText),
                      child: ListTile(
                        leading: Icon(Icons.photo),
                        title: Text("Slike"),
                        trailing: Icon(Icons.file_upload),
                        onTap: () => getImages(true),
                      ),
                    );
                  }),
                ),
                if (widget.displayImagesValidationMsg) ...[
                  Row(
                    children: [
                      Padding(
                        padding: EdgeInsets.fromLTRB(0, 5, 0, 5),
                        child: Text(
                          "Molimo odaberite minimalno jednu sliku",
                          style: TextStyle(
                            fontSize: 12.0,
                            color: Colors.red[700],
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
                if (widget.images != null && widget.images!.isNotEmpty) ...[
                  Wrap(
                    alignment: WrapAlignment.start,
                    spacing: 8.0,
                    runSpacing: 8.0,
                    children: List<Widget>.generate(widget.images!.length, (index) {
                      return Row(
                        mainAxisSize: MainAxisSize.min,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Container(
                            padding: EdgeInsets.fromLTRB(0, 15, 0, 0),
                            width: 100,
                            height: 100,
                            child: Image.memory(base64Decode(widget.images![index].image)),
                          ),
                          IconButton(
                            icon: Icon(Icons.close),
                            onPressed: () {
                              setState(() {
                                widget.images?.removeAt(index);
                                widget.onUpdateImages(widget.images!);
                              });
                            },
                          ),
                        ],
                      );
                    }),
                  ),
                ]
              ])),
        ],
      ),
      const SizedBox(height: 20),
    ]);
  }
}
